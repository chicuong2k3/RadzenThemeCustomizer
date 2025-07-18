namespace RadzenThemeCustomizer.Shared;

public class GetThemesRequest
{
    public int PageSize { get; set; } = 5;
    public int PageNumber { get; set; } = 1;
    public string? SearchTerm { get; set; }
}
