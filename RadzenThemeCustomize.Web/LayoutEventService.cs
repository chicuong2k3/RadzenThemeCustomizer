namespace RadzenThemeCustomizer.Web;

public class LayoutEventService
{
    public event Func<Task>? OnChangeTheme;

    public async Task TriggerChangeTheme()
    {
        if (OnChangeTheme != null)
        {
            await OnChangeTheme.Invoke();
        }
    }
}
