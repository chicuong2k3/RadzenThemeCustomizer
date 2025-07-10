namespace RadzenThemeCustomizer.Web;

public class ColorVariableService
{
    private List<VariableOption> _availableVariables = new()
    {
        new VariableOption { VariableName = "rz-white", DisplayName = "White" },
        new VariableOption { VariableName = "rz-dark", DisplayName = "Dark" },
        new VariableOption { VariableName = "rz-primary", DisplayName = "Primary" },
        new VariableOption { VariableName = "rz-secondary", DisplayName = "Secondary" },
        new VariableOption { VariableName = "rz-info", DisplayName = "Info" },
        new VariableOption { VariableName = "rz-success", DisplayName = "Success" },
        new VariableOption { VariableName = "rz-warning", DisplayName = "Warning" },
        new VariableOption { VariableName = "rz-danger", DisplayName = "Danger" },
        new VariableOption { VariableName = "rz-base-50", DisplayName = "Base 50" },
        new VariableOption { VariableName = "rz-base-100", DisplayName = "Base 100" },
        new VariableOption { VariableName = "rz-base-200", DisplayName = "Base 200" },
        new VariableOption { VariableName = "rz-base-300", DisplayName = "Base 300" },
        new VariableOption { VariableName = "rz-base-400", DisplayName = "Base 400" },
        new VariableOption { VariableName = "rz-base-500", DisplayName = "Base 500" },
        new VariableOption { VariableName = "rz-base-600", DisplayName = "Base 600" },
        new VariableOption { VariableName = "rz-base-700", DisplayName = "Base 700" },
        new VariableOption { VariableName = "rz-base-800", DisplayName = "Base 800" },
        new VariableOption { VariableName = "rz-base-900", DisplayName = "Base 900" },
        new VariableOption { VariableName = "rz-base-light", DisplayName = "Base Light" },
        new VariableOption { VariableName = "rz-base-lighter", DisplayName = "Base Lighter" },
        new VariableOption { VariableName = "rz-base-dark", DisplayName = "Base Dark" },
        new VariableOption { VariableName = "rz-base-darker", DisplayName = "Base Darker" },
    };

    public List<VariableOption> GetColorVariables()
    {
        return _availableVariables;
    }
}
