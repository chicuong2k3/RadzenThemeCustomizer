namespace RadzenThemeCustomizer.Shared;

public class CreateThemeRequest
{
    public string ThemeName { get; set; } = string.Empty;
    public string BaseTheme { get; set; } = "default";
}
