namespace RadzenThemeCustomizer.Shared;
public class UpdateThemeRequest
{
    public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();
    public string ThemeName { get; set; } = string.Empty;
}