namespace RadzenThemeCustomizer.Api;

public class User
{
    public string Id { get; set; } = string.Empty;
    public ICollection<Theme> Themes { get; set; } = [];
}
