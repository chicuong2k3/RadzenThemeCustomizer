namespace RadzenThemeCustomizer.Web;

public class LayoutEventService
{
    public string CurrentTheme { get; private set; } = string.Empty;

    public event Func<Task>? OnChangeTheme;

    public async Task SetThemeAsync(string newTheme)
    {
        CurrentTheme = newTheme;

        if (OnChangeTheme != null)
        {
            await OnChangeTheme.Invoke();
        }
    }

    public async Task TriggerChangeTheme()
    {
        if (OnChangeTheme != null)
        {
            await OnChangeTheme.Invoke();
        }
    }
}
