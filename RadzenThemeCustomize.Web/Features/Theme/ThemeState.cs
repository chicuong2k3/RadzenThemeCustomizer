using TimeWarp.State;

namespace RadzenThemeCustomizer.Web.Features.Theme;

public sealed partial class ThemeState : State<ThemeState>
{
    public string CurrentTheme { get; private set; } = string.Empty;

    public bool IsLoading { get; private set; }

    public string BaseColor { get; private set; } = string.Empty;
    public string PrimaryColor { get; private set; } = string.Empty;
    public string SecondaryColor { get; private set; } = string.Empty;
    public string InfoColor { get; private set; } = string.Empty;
    public string SuccessColor { get; private set; } = string.Empty;
    public string WarningColor { get; private set; } = string.Empty;
    public string DangerColor { get; private set; } = string.Empty;

    public Dictionary<string, string> BaseColors = new()
    {
        { "rz-base-50", string.Empty },
        { "rz-base-100", string.Empty },
        { "rz-base-200", string.Empty },
        { "rz-base-300", string.Empty },
        { "rz-base-400", string.Empty },
        { "rz-base-500", string.Empty },
        { "rz-base-600", string.Empty },
        { "rz-base-700", string.Empty },
        { "rz-base-800", string.Empty },
        { "rz-base-900", string.Empty },
        { "rz-base-light", string.Empty },
        { "rz-base-lighter", string.Empty },
        { "rz-base-dark", string.Empty },
        { "rz-base-darker", string.Empty }
    };

    public Dictionary<string, string> SeriesColors = new()
    {
        { "rz-series-1", string.Empty },
        { "rz-series-2", string.Empty },
        { "rz-series-3", string.Empty },
        { "rz-series-4", string.Empty },
        { "rz-series-5", string.Empty },
        { "rz-series-6", string.Empty },
        { "rz-series-7", string.Empty },
        { "rz-series-8", string.Empty },
        { "rz-series-9", string.Empty },
        { "rz-series-10", string.Empty },
        { "rz-series-11", string.Empty },
        { "rz-series-12", string.Empty },
        { "rz-series-13", string.Empty },
        { "rz-series-14", string.Empty },
        { "rz-series-15", string.Empty },
        { "rz-series-16", string.Empty },
        { "rz-series-17", string.Empty },
        { "rz-series-18", string.Empty },
        { "rz-series-19", string.Empty },
        { "rz-series-20", string.Empty },
        { "rz-series-21", string.Empty },
        { "rz-series-22", string.Empty },
        { "rz-series-23", string.Empty },
        { "rz-series-24", string.Empty }
    };



    public Dictionary<string, ThemeVariable> LayoutVariables = new()
    {
        { "rz-border-width", new() },
        { "rz-border-radius", new() },
        { "rz-root-font-size", new() },
        { "rz-body-font-size", new() },
        { "rz-body-line-height", new() },
        { "rz-text-font-family", new() },
        { "rz-outline-offset", new() },
        { "rz-outline-width", new() },
        { "rz-outline-color", new() },
        { "rz-body-background-color", new() }
    };

    public Dictionary<string, ShadowVariable> ShadowVariables = new()
    {
        { "rz-shadow-1", new() },
        { "rz-shadow-2", new() },
        { "rz-shadow-3", new() },
        { "rz-shadow-4", new() },
        { "rz-shadow-5", new() },
        { "rz-shadow-6", new() },
        { "rz-shadow-7", new() },
        { "rz-shadow-8", new() },
        { "rz-shadow-9", new() },
        { "rz-shadow-10", new() },
        { "rz-shadow-inset-level-1", new() }
    };

    public Dictionary<string, string> SemanticColors = new()
    {
        { "rz-text-title-color", string.Empty },
        { "rz-text-color", string.Empty },
        { "rz-text-secondary-color", string.Empty },
        { "rz-text-tertiary-color", string.Empty },
        { "rz-text-disabled-color", string.Empty },
        { "rz-text-contrast-color", string.Empty },
        { "rz-border-normal", string.Empty },
        { "rz-border-hover", string.Empty },
        { "rz-border-focus", string.Empty },
        { "rz-border-disabled", string.Empty }
    };


    public override void Initialize()
    {
    }
}
