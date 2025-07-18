namespace RadzenThemeCustomizer.Shared;
public class UpdateThemeRequest
{
    public Dictionary<string, string> Properties { get; set; } = new();
    public string ThemeName { get; set; } = string.Empty;
}