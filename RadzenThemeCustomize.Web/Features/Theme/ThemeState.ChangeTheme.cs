using Microsoft.JSInterop;
using Radzen.Blazor.Rendering;
using System.Text;
using System.Text.RegularExpressions;
using TimeWarp.State;

namespace RadzenThemeCustomizer.Web.Features.Theme;

public partial class ThemeState
{
    public static class ChangeThemeActionSet
    {
        public sealed class Action : IAction
        {
            public string ThemeName { get; }
            public Action(string themeName)
            {
                ThemeName = themeName;
            }
        }

        public sealed class Handler : ActionHandler<Action>
        {
            private readonly ThemeManagerService _themeManagerService;
            private readonly ColorVariableService _colorVariableService;
            private readonly IJSRuntime _jSRuntime;

            private ThemeState ThemeState => Store.GetState<ThemeState>();

            public Handler(
                ThemeManagerService themeManagerService,
                ColorVariableService colorVariableService,
                IJSRuntime jSRuntime,
                IStore store) : base(store)
            {
                _themeManagerService = themeManagerService;
                _colorVariableService = colorVariableService;
                _jSRuntime = jSRuntime;
            }

            public override async Task Handle(Action action, CancellationToken cancellationToken)
            {
                ThemeState.CurrentTheme = action.ThemeName;

                ThemeState.BaseColor = await _themeManagerService.GetScssVariableAsync(new()
                {
                    VariableName = "rz-base",
                    ThemeName = ThemeState.CurrentTheme
                }) ?? string.Empty; ;

                ThemeState.PrimaryColor = await _themeManagerService.GetScssVariableAsync(new()
                {
                    VariableName = "rz-primary",
                    ThemeName = ThemeState.CurrentTheme
                }) ?? string.Empty;

                ThemeState.SecondaryColor = await _themeManagerService.GetScssVariableAsync(new()
                {
                    VariableName = "rz-secondary",
                    ThemeName = ThemeState.CurrentTheme
                }) ?? string.Empty;

                ThemeState.InfoColor = await _themeManagerService.GetScssVariableAsync(new()
                {
                    VariableName = "rz-info",
                    ThemeName = ThemeState.CurrentTheme
                }) ?? string.Empty;

                ThemeState.SuccessColor = await _themeManagerService.GetScssVariableAsync(new()
                {
                    VariableName = "rz-success",
                    ThemeName = ThemeState.CurrentTheme
                }) ?? string.Empty;

                ThemeState.WarningColor = await _themeManagerService.GetScssVariableAsync(new()
                {
                    VariableName = "rz-warning",
                    ThemeName = ThemeState.CurrentTheme
                }) ?? string.Empty;

                ThemeState.DangerColor = await _themeManagerService.GetScssVariableAsync(new()
                {
                    VariableName = "rz-danger",
                    ThemeName = ThemeState.CurrentTheme
                }) ?? string.Empty;



                foreach (var key in ThemeState.BaseColors.Keys)
                {
                    var value = await _themeManagerService.GetScssVariableAsync(new()
                    {
                        VariableName = key,
                        ThemeName = ThemeState.CurrentTheme
                    });
                    ThemeState.BaseColors[key] = value;
                }


                foreach (var key in ThemeState.SeriesColors.Keys)
                {
                    var value = await _themeManagerService.GetScssVariableAsync(new()
                    {
                        VariableName = key,
                        ThemeName = ThemeState.CurrentTheme
                    });
                    ThemeState.SeriesColors[key] = value;
                }


                var availableVariables = _colorVariableService.GetColorVariables();
                foreach (var key in ThemeState.LayoutVariables.Keys)
                {
                    var value = await _themeManagerService.GetScssVariableAsync(new()
                    {
                        VariableName = key,
                        ThemeName = ThemeState.CurrentTheme
                    });

                    if (string.IsNullOrEmpty(value))
                    {
                        continue;
                    }

                    if (key == "rz-outline-color"
                        || key == "rz-body-background-color")
                    {
                        ThemeState.LayoutVariables[key].Value = value.Substring(6, value.Length - 7);
                        continue;
                    }
                    else if (key == "rz-text-font-family")
                    {
                        ThemeState.LayoutVariables[key].Value = value;
                        continue;
                    }
                    else
                    {
                        var numericPart = Regex.Replace(value, "[^0-9.]", "");
                        var unitPart = Regex.Match(value, "[a-zA-Z]+$")?.Value ?? "px";
                        if (double.TryParse(numericPart, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var number))
                        {
                            ThemeState.LayoutVariables[key].Value = number.ToString();
                            ThemeState.LayoutVariables[key].Unit = unitPart;
                        }
                    }
                }


                var regex = new Regex(@"var\(--([a-zA-Z0-9\-]+)\)");
                foreach (var key in ThemeState.SemanticColors.Keys)
                {
                    var value = await _themeManagerService.GetScssVariableAsync(new()
                    {
                        VariableName = key,
                        ThemeName = ThemeState.CurrentTheme
                    });

                    if (string.IsNullOrEmpty(value))
                    {
                        continue;
                    }

                    if (key.Contains("border"))
                    {
                        var matches = regex.Matches(value);
                        if (matches.Count > 0)
                        {
                            var lastMatch = matches[matches.Count - 1];
                            var variableValue = lastMatch.Groups[1].Value;
                            ThemeState.SemanticColors[key] = variableValue;
                        }
                    }
                    else
                    {

                        ThemeState.SemanticColors[key] = value.Substring(6, value.Length - 7);
                    }
                }


                foreach (var key in ThemeState.ShadowVariables.Keys)
                {
                    var value = await _themeManagerService.GetScssVariableAsync(new()
                    {
                        VariableName = key,
                        ThemeName = ThemeState.CurrentTheme
                    });

                    var firstValue = GetFirstBoxShadow(value);

                    var parts = SplitShadowValues(firstValue)
                                    .Select(x => x.Replace("px", "").Trim())
                                    .ToList();


                    if (parts.Count < 4)
                    {
                        continue;
                    }

                    if (int.TryParse(parts[0], out var offsetX))
                        ThemeState.ShadowVariables[key].OffsetX = offsetX;

                    if (int.TryParse(parts[1], out var offsetY))
                        ThemeState.ShadowVariables[key].OffsetY = offsetY;

                    if (int.TryParse(parts[2], out var blurRadius))
                        ThemeState.ShadowVariables[key].BlurRadius = blurRadius;

                    if (parts.Count >= 5 && int.TryParse(parts[3], out var spreadRadius))
                        ThemeState.ShadowVariables[key].SpreadRadius = spreadRadius;
                    else
                        ThemeState.ShadowVariables[key].SpreadRadius = 0;

                    var color = parts[^1];
                    ThemeState.ShadowVariables[key].Color = color;
                }

                var css = await _themeManagerService.GetThemeCssAsync(new()
                {
                    ThemeName = ThemeState.CurrentTheme
                });
                await _jSRuntime.InvokeVoidAsync("injectCss", css);
            }
        }

        private static string GetFirstBoxShadow(string input)
        {
            int parenDepth = 0;
            var current = new StringBuilder();

            foreach (char c in input)
            {
                if (c == '(') parenDepth++;
                else if (c == ')') parenDepth--;

                if (c == ',' && parenDepth == 0)
                {
                    break;
                }

                current.Append(c);
            }

            return current.ToString().Trim();
        }

        private static List<string> SplitShadowValues(string shadow)
        {
            var parts = new List<string>();
            var current = new StringBuilder();
            int parenDepth = 0;

            foreach (char c in shadow)
            {
                if (c == '(') parenDepth++;
                if (c == ')') parenDepth--;

                if (c == ' ' && parenDepth == 0)
                {
                    if (current.Length > 0)
                    {
                        parts.Add(current.ToString());
                        current.Clear();
                    }
                }
                else
                {
                    current.Append(c);
                }
            }

            if (current.Length > 0)
                parts.Add(current.ToString());

            return parts;
        }
    }
}
