using Microsoft.AspNetCore.Mvc;
using RadzenThemeCustomizer.Api;
using System.Text;

namespace RadzenThemeCustomizer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ThemeController : ControllerBase
{
    private readonly ThemeManagerService _themeManagerService;

    public ThemeController(ThemeManagerService themeManagerService)
    {
        _themeManagerService = themeManagerService;
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateTheme([FromBody] Dictionary<string, string> properties)
    {
        try
        {
            await _themeManagerService.UpdateThemeAsync(properties);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error updating theme: {ex.Message}");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetThemeCss()
    {
        var cssContent = await _themeManagerService.GetThemeCssAsync();

        return Content(cssContent, "text/css", Encoding.UTF8);
    }

    [HttpGet("variable")]
    public async Task<IActionResult> GetVariable([FromQuery] string name)
    {
        var variable = await _themeManagerService.GetScssVariableAsync(name);
        if (!string.IsNullOrEmpty(variable))
        {
            return Ok(variable);
        }
        return NotFound();
    }

    // POST: api/theme/reset
    [HttpPost("reset")]
    public async Task<IActionResult> ResetTheme([FromBody] ResetThemeRequest request)
    {
        try
        {
            await _themeManagerService.ResetThemeAsync(request.ThemeName);

            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error reseting theme: {ex.Message}");
        }
    }

    // GET: api/theme/css-file
    [HttpGet("css-file")]
    public async Task<IActionResult> GetCssFile()
    {
        try
        {

            var bytes = await _themeManagerService.GetCssFileAsync();
            return File(bytes, "text/css", "theme.css");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error getting css file: {ex.Message}");
        }
    }

    public class ResetThemeRequest
    {
        public string ThemeName { get; set; } = string.Empty;
    }
}