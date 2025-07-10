using DartSassHost;
using JavaScriptEngineSwitcher.Core;
using JavaScriptEngineSwitcher.V8;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RadzenThemeCustomizer.Api;

public class ThemeManagerService
{
    private string _themeName = "standard";
    private readonly string _scssFilePath = Path.Combine(Directory.GetCurrentDirectory(), "scss", "generated", "theme-base.scss");
    private readonly string _cssFilePath = Path.Combine(Directory.GetCurrentDirectory(), "css", "theme-base.css");
    private readonly string[] _includePaths =
    [
        Path.Combine(Directory.GetCurrentDirectory(), "scss"),
        Path.Combine(Directory.GetCurrentDirectory(), "scss", "components")
    ];

    public ThemeManagerService()
    {
        var jsEngineSwitcher = JsEngineSwitcher.Current;
    }

    public async Task UpdateThemeAsync(Dictionary<string, string> properties)
    {
        try
        {
            var templateFilePath = Path.Combine(Directory.GetCurrentDirectory(), "scss", $"{_themeName}.scss");
            var templateContent = await File.ReadAllTextAsync(templateFilePath);
            var scssContent = UpdateProperties(templateContent, properties);

            await SafeWriteFileAsync(_scssFilePath, scssContent);

            using var compiler = new SassCompiler();
            var result = compiler.CompileFile(_scssFilePath, options: new CompilationOptions
            {
                IncludePaths = _includePaths,
                OutputStyle = OutputStyle.Expanded
            });

            var css = result.CompiledContent;

            if (!Directory.Exists(Path.GetDirectoryName(_cssFilePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_cssFilePath)!);
            }

            await SafeWriteFileAsync(_cssFilePath, css);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to update theme: {ex.Message}", ex);
        }
    }

    public async Task<string> GetThemeCssAsync()
    {
        if (!File.Exists(_cssFilePath))
        {
            await UpdateThemeAsync(new Dictionary<string, string>());
        }

        return await SafeReadFileAsync(_cssFilePath);
    }

    private async Task<string> GetScssContentAsync()
    {
        if (!File.Exists(_scssFilePath))
        {
            await UpdateThemeAsync(new Dictionary<string, string>());
        }

        return await SafeReadFileAsync(_scssFilePath);
    }

    public async Task<string> GetScssVariableAsync(string variableName)
    {
        try
        {
            var scssContent = await GetScssContentAsync();
            var regex = new Regex($@"\${Regex.Escape(variableName)}\s*:\s*([^;]+?)(?:\s*!default)?;", RegexOptions.Multiline);
            var match = regex.Match(scssContent);

            if (match.Success)
            {
                return match.Groups[1].Value.Trim();
            }

            Console.WriteLine($"SCSS variable ${variableName} not found in standard.scss");
            return string.Empty;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting SCSS variable ${variableName}: {ex.Message}");
            return string.Empty;
        }
    }

    public async Task ResetThemeAsync(string themeName)
    {
        await SafeDeleteFileAsync(_scssFilePath);
        await SafeDeleteFileAsync(_cssFilePath);
        _themeName = themeName;
        await UpdateThemeAsync(new Dictionary<string, string>());
    }

    private string UpdateProperties(string scssContent, Dictionary<string, string> properties)
    {
        var regex = new Regex(
            @"(\$[^:\s]+)\s*:\s*([^;]+?)(?:\s*!default)?;",
            RegexOptions.Multiline
        );

        scssContent = regex.Replace(scssContent, match =>
        {
            string variableName = match.Groups[1].Value;
            string key = variableName.Substring(1);
            bool hasDefault = match.Value.Contains("!default");

            if (properties.TryGetValue(key, out var newValue))
            {
                Console.WriteLine($"Updating SCSS variable {variableName} to: {newValue}");
                return $"{variableName}: {newValue}{(hasDefault ? " !default" : "")};";
            }

            return match.Value;
        });

        return scssContent;
    }

    private async Task<string> SafeReadFileAsync(string path, int retries = 5)
    {
        if (!File.Exists(path))
        {
            Console.WriteLine($"File not found: {path}");
            return string.Empty;
        }

        for (int i = 0; i < retries; i++)
        {
            try
            {
                using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using var reader = new StreamReader(stream);
                return await reader.ReadToEndAsync();
            }
            catch (IOException) when (i < retries - 1)
            {
                await Task.Delay(100);
            }
        }

        throw new IOException($"Failed to read file after {retries} attempts: {path}");
    }

    private async Task SafeWriteFileAsync(string path, string content, int retries = 5)
    {
        for (int i = 0; i < retries; i++)
        {
            try
            {
                if (File.Exists(path))
                {
                    await SafeDeleteFileAsync(path);
                }

                using var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
                using var writer = new StreamWriter(stream);
                await writer.WriteAsync(content);
                return;
            }
            catch (IOException) when (i < retries - 1)
            {
                await Task.Delay(100);
            }
        }

        throw new IOException($"Failed to write file after {retries} attempts: {path}");
    }

    private async Task SafeDeleteFileAsync(string path, int retries = 5)
    {
        if (!File.Exists(path))
        {
            return;
        }

        for (int i = 0; i < retries; i++)
        {
            try
            {
                File.Delete(path);
                return;
            }
            catch (IOException) when (i < retries - 1)
            {
                await Task.Delay(100);
            }
        }

        throw new IOException($"Failed to delete file after {retries} attempts: {path}");
    }

    public async Task<byte[]> GetCssFileAsync()
    {
        if (!File.Exists(_cssFilePath))
        {
            throw new FileNotFoundException("CSS file not found. Please ensure the theme has been generated.");
        }
        return await File.ReadAllBytesAsync(_cssFilePath);
    }

}