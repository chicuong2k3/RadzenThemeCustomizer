using Microsoft.JSInterop;
using TimeWarp.State;

namespace RadzenThemeCustomizer.Web.Features.Theme;

public partial class ThemeState
{
    public static class ChangeAccentColorActionSet
    {
        public sealed class Action : IAction
        {
            public string Variable { get; }
            public string Value { get; }

            public Action(string variable, string value)
            {
                Variable = variable;
                Value = value;
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
                switch (action.Variable)
                {
                    case "rz-base":
                        ThemeState.BaseColor = action.Value;
                        break;
                    case "rz-primary":
                        ThemeState.PrimaryColor = action.Value;
                        break;
                    case "rz-secondary":
                        ThemeState.SecondaryColor = action.Value;
                        break;
                    case "rz-info":
                        ThemeState.InfoColor = action.Value;
                        break;
                    case "rz-success":
                        ThemeState.SuccessColor = action.Value;
                        break;
                    case "rz-warning":
                        ThemeState.WarningColor = action.Value;
                        break;
                    case "rz-danger":
                        ThemeState.DangerColor = action.Value;
                        break;
                    default: break;
                }

                await _themeManagerService.UpdateThemeAsync(new()
                {
                    ThemeName = ThemeState.CurrentTheme,
                    Properties = { { action.Variable, action.Value } }
                });
                var css = await _themeManagerService.GetThemeCssAsync(new()
                {
                    ThemeName = ThemeState.CurrentTheme
                });
                await _jSRuntime.InvokeVoidAsync("injectCss", css);

            }
        }
    }
}
