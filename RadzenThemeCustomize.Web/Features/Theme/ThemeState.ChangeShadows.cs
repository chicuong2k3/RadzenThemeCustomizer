using Microsoft.JSInterop;
using TimeWarp.State;

namespace RadzenThemeCustomizer.Web.Features.Theme;

public partial class ThemeState
{
    public static class ChangeShadowsActionSet
    {
        public sealed class Action : IAction
        {
            public Action()
            {
            }
        }

        public sealed class Handler : ActionHandler<Action>
        {
            private readonly ThemeManagerService _themeManagerService;
            private readonly IJSRuntime _jSRuntime;

            public Handler(
                IStore store,
                ThemeManagerService themeManagerService,
                IJSRuntime jSRuntime) : base(store)
            {
                _themeManagerService = themeManagerService;
                _jSRuntime = jSRuntime;
            }

            private ThemeState ThemeState => Store.GetState<ThemeState>();

            public override async Task Handle(Action action, CancellationToken cancellationToken)
            {
                ThemeState.IsLoading = true;

                Dictionary<string, string> shadows = new();
                foreach (var shadowVariable in ThemeState.ShadowVariables)
                {
                    Console.WriteLine(shadowVariable.Value.Color);
                    shadows[shadowVariable.Key] = $"{shadowVariable.Value.OffsetX.ToString()}px {shadowVariable.Value.OffsetY.ToString()}px {shadowVariable.Value.BlurRadius.ToString()}px {shadowVariable.Value.SpreadRadius.ToString()}px {shadowVariable.Value.Color}";
                }

                await _themeManagerService.UpdateThemeAsync(new()
                {
                    ThemeName = ThemeState.CurrentTheme,
                    Properties = shadows
                });

                var css = await _themeManagerService.GetThemeCssAsync(new()
                {
                    ThemeName = ThemeState.CurrentTheme
                });
                await _jSRuntime.InvokeVoidAsync("injectCss", css);

                ThemeState.IsLoading = false;
            }
        }
    }
}
