using DartSassHost;
using JavaScriptEngineSwitcher.Core;
using Microsoft.EntityFrameworkCore;
using RadzenThemeCustomizer.Shared;
using System.Text;
using System.Text.RegularExpressions;

namespace RadzenThemeCustomizer.Api;

public class ThemeManagerService
{
    private readonly string[] _includePaths =
    [
        Path.Combine(Directory.GetCurrentDirectory(), "scss"),
        Path.Combine(Directory.GetCurrentDirectory(), "scss", "components")
    ];
    private readonly RadzenThemeCustomizerDbContext _dbContext;

    public ThemeManagerService(RadzenThemeCustomizerDbContext dbContext)
    {
        var jsEngineSwitcher = JsEngineSwitcher.Current;
        _dbContext = dbContext;
    }

    public async Task<Theme?> GetSingleThemeAsync(string userId)
    {
        return await _dbContext.Themes
            .FirstOrDefaultAsync(t => t.UserId == userId);
    }

    public async Task<PaginationResult<Theme>> GetThemesAsync(string userId, int pageNumber, int pageSize)
    {
        var totalCount = await _dbContext.Themes
            .CountAsync(t => t.UserId == userId);

        var items = await _dbContext.Themes
            .Where(t => t.UserId == userId)
            .OrderBy(t => t.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginationResult<Theme>()
        {
            TotalCount = totalCount,
            PageSize = pageSize,
            PageNumber = pageNumber,
            Items = items
        };
    }

    public async Task DeleteThemeAsync(string themeName, string userId)
    {
        var theme = await _dbContext.Themes
            .FirstOrDefaultAsync(t => t.Name == themeName && t.UserId == userId);
        if (theme == null)
        {
            throw new InvalidOperationException($"Theme '{themeName}' not found for user '{userId}'.");
        }
        _dbContext.Themes.Remove(theme);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Theme> CreateThemeAsync(string themeName, string userId, string baseTheme)
    {
        var existingTheme = await _dbContext.Themes
            .FirstOrDefaultAsync(t => t.Name == themeName && t.UserId == userId);
        if (existingTheme != null)
        {
            throw new InvalidOperationException($"Theme '{themeName}' already exists for user '{userId}'.");
        }

        var baseThemePath = Path.Combine(Directory.GetCurrentDirectory(), "scss", $"{baseTheme}.scss");

        var scssContent = File.ReadAllText(baseThemePath);

        using var compiler = new SassCompiler();
        var result = compiler.CompileFile(baseThemePath, options: new CompilationOptions
        {
            IncludePaths = _includePaths,
            OutputStyle = OutputStyle.Expanded
        });
        var theme = new Theme
        {
            Name = themeName,
            UserId = userId,
            ScssContent = scssContent,
            CssContent = result.CompiledContent
        };
        _dbContext.Themes.Add(theme);
        await _dbContext.SaveChangesAsync();

        return theme;

    }

    public async Task UpdateThemeAsync(Dictionary<string, string> properties, string themeName, string userId)
    {
        var theme = await _dbContext.Themes
        .FirstOrDefaultAsync(t => t.Name == themeName && t.UserId == userId);

        if (theme == null)
        {
            return;
        }

        var scssContent = theme.ScssContent;
        var updatedScssContent = UpdateProperties(scssContent, properties);

        using var compiler = new SassCompiler();
        var result = compiler.Compile(updatedScssContent, false, options: new CompilationOptions
        {
            IncludePaths = _includePaths,
            OutputStyle = OutputStyle.Expanded
        });

        theme.ScssContent = updatedScssContent;
        theme.CssContent = result.CompiledContent;
        await _dbContext.SaveChangesAsync();

    }

    public async Task<string> GetThemeCssAsync(string themeName, string userId)
    {
        var theme = await _dbContext.Themes
            .FirstOrDefaultAsync(t => t.Name == themeName && t.UserId == userId);

        if (theme == null)
        {
            return string.Empty;
        }

        return theme.CssContent;
    }

    public async Task<string> GetScssVariableAsync(string variableName, string themeName, string userId)
    {
        var theme = await _dbContext.Themes
            .FirstOrDefaultAsync(t => t.Name == themeName && t.UserId == userId);

        if (theme == null)
        {
            return string.Empty;
        }

        var scssContent = theme.ScssContent;
        var regex = new Regex($@"\${Regex.Escape(variableName)}\s*:\s*([^;]+?)(?:\s*!default)?;", RegexOptions.Multiline);
        var match = regex.Match(scssContent);

        if (match.Success)
        {
            return match.Groups[1].Value.Trim();
        }

        Console.WriteLine($"SCSS variable ${variableName} not found in theme {theme.Name}");
        return string.Empty;

    }

    private string UpdateProperties(string scssContent, Dictionary<string, string> properties)
    {
        var regex = new Regex(
            @"(\$[^:\s]+)\s*:\s*([^;]+?)(?:\s*!default)?;",
            RegexOptions.Multiline
        );

        var existingKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        scssContent = regex.Replace(scssContent, match =>
        {
            string variableName = match.Groups[1].Value;
            string key = variableName.Substring(1);
            bool hasDefault = match.Value.Contains("!default");

            existingKeys.Add(key);

            if (properties.TryGetValue(key, out var newValue))
            {
                if (string.IsNullOrWhiteSpace(newValue))
                {
                    return match.Value;
                }

                return $"{variableName}: {newValue}{(hasDefault ? " !default" : "")};";
            }

            return match.Value;
        });

        var newLines = new List<string>();
        foreach (var kvp in properties)
        {
            if (!existingKeys.Contains(kvp.Key) && !string.IsNullOrWhiteSpace(kvp.Value))
            {
                string variableName = $"${kvp.Key}";
                string newLine = $"{variableName}: {kvp.Value} !default;";
                newLines.Add(newLine);
            }
        }

        if (newLines.Count > 0)
        {
            scssContent += Environment.NewLine + string.Join(Environment.NewLine, newLines);
        }

        return scssContent;
    }


    public async Task<byte[]> GetCssFileAsync(string themeName, string userId)
    {
        var theme = await _dbContext.Themes
            .FirstOrDefaultAsync(t => t.Name == themeName && t.UserId == userId);

        if (theme == null)
        {
            return [];
        }

        var cssBytes = Encoding.UTF8.GetBytes(theme.CssContent);

        return cssBytes;
    }

}