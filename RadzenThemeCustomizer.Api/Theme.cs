using RadzenThemeCustomizer.Shared;

namespace RadzenThemeCustomizer.Api;

public class Theme
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ScssContent { get; set; } = string.Empty;
    public string CssContent { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
}


public static class ThemeDtoExtensions
{
    public static ThemeDto ToDto(this Theme theme)
    {
        return new ThemeDto
        {
            Name = theme.Name,
            UserId = theme.UserId
        };
    }
}
