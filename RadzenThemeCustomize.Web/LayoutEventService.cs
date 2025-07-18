using System.Data;

namespace RadzenThemeCustomizer.Web;

public class LayoutEventService
{
    public string CurrentTheme { get; private set; } = string.Empty;
    private bool isUpdating = false; private DateTime lastUpdate = DateTime.MinValue;
    private readonly TimeSpan debounceDelay = TimeSpan.FromMilliseconds(500);
    public event Func<Task>? OnChangeTheme;
    public event Func<Task>? OnUpdateTheme;

    public async Task SetThemeAsync(string newTheme)
    {
        if ((DateTime.UtcNow - lastUpdate) < debounceDelay)
            return;

        CurrentTheme = newTheme;
        if (OnChangeTheme != null)
        {
            Console.WriteLine($"OnChangeTheme triggered for theme: {newTheme}");
            await OnChangeTheme.Invoke();
        }
        lastUpdate = DateTime.UtcNow;
    }

    public async Task TriggerUpdateTheme()
    {
        if (isUpdating) return;
        isUpdating = true;

        try
        {
            if (OnUpdateTheme != null)
            {
                await OnUpdateTheme.Invoke();
            }
        }
        finally
        {
            isUpdating = false;
        }
    }
}
