

namespace RadzenThemeCustomizer.Shared;

public class PaginationResult<T>
{
    public int TotalCount { get; set; } = 0;
    public int PageSize { get; set; } = 10;
    public int PageNumber { get; set; } = 1;
    public List<T> Items { get; set; } = [];
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);



}
