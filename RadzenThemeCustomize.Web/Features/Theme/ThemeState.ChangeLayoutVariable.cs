using Microsoft.JSInterop;
using TimeWarp.State;

namespace RadzenThemeCustomizer.Web.Features.Theme;

public partial class ThemeState
{
    public static class ChangeLayoutVariableActionSet
    {
        public sealed class Action : IAction
        {
            public string Variable { get; }
            public ThemeVariable Value { get; }

            public Action(string variable, ThemeVariable value)
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
                ThemeState.IsLoading = true;

                ThemeState.LayoutVariables[action.Variable] = action.Value;

                await _themeManagerService.UpdateThemeAsync(new()
                {
                    ThemeName = ThemeState.CurrentTheme,
                    Properties =
                    {
                        {
                            action.Variable,
                            action.Value.IsVar ? $"var(--{action.Value.Value})" : $"{action.Value.Value}{action.Value.Unit}"
                        }
                    }
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
