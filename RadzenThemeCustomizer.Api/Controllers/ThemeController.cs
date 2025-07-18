using Microsoft.AspNetCore.Mvc;
using RadzenThemeCustomizer.Shared;
using System.Text;

namespace RadzenThemeCustomizer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public partial class ThemeController : ControllerBase
{
    private readonly ThemeManagerService _themeManagerService;

    public ThemeController(ThemeManagerService themeManagerService)
    {
        _themeManagerService = themeManagerService;
    }

    [HttpGet("single")]
    public async Task<IActionResult> GetSingleTheme()
    {
        var theme = await _themeManagerService.GetSingleThemeAsync(HttpContext.GetUserId());
        if (theme == null)
        {
            return NotFound("Theme not found.");
        }
        return Ok(theme.ToDto());
    }

    [HttpGet]
    public async Task<IActionResult> GetThemes([FromQuery] GetThemesRequest request)
    {
        var result = await _themeManagerService.GetThemesAsync(HttpContext.GetUserId(), request.PageNumber, request.PageSize);
        return Ok(new PaginationResult<ThemeDto>()
        {
            TotalCount = result.TotalCount,
            PageSize = result.PageSize,
            PageNumber = result.PageNumber,
            Items = result.Items.Select(t => t.ToDto()).ToList()
        });
    }

    [HttpDelete("{themeName}")]
    public async Task<IActionResult> DeleteTheme(string themeName)
    {
        await _themeManagerService.DeleteThemeAsync(themeName, HttpContext.GetUserId());
        return NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> CreateTheme([FromBody] CreateThemeRequest request)
    {
        var createdTheme = await _themeManagerService.CreateThemeAsync(request.ThemeName, HttpContext.GetUserId(), request.BaseTheme);
        return Ok(createdTheme.ToDto());
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTheme([FromBody] UpdateThemeRequest request)
    {
        await _themeManagerService.UpdateThemeAsync(request.Properties, request.ThemeName, HttpContext.GetUserId());
        return Ok();
    }

    [HttpGet("css")]
    public async Task<IActionResult> GetThemeCss([FromQuery] GetThemeCssRequest request)
    {
        var cssContent = await _themeManagerService.GetThemeCssAsync(request.ThemeName, HttpContext.GetUserId());

        if (string.IsNullOrEmpty(cssContent))
        {
            return NotFound("Theme not found or CSS content is empty.");
        }

        return Content(cssContent, "text/css", Encoding.UTF8);
    }

    [HttpGet("variable")]
    public async Task<IActionResult> GetVariable([FromQuery] GetVariableRequest request)
    {
        var variable = await _themeManagerService.GetScssVariableAsync(request.VariableName, request.ThemeName, HttpContext.GetUserId());
        if (!string.IsNullOrEmpty(variable))
        {
            return Ok(variable);
        }
        return NotFound();
    }

    [HttpPost("css-file")]
    public async Task<IActionResult> DownloadCssFile([FromBody] DownloadCssFileRequest request)
    {
        var bytes = await _themeManagerService.GetCssFileAsync(request.ThemeName, HttpContext.GetUserId());

        if (bytes == null || bytes.Length == 0)
        {
            return NotFound("Theme not found or CSS file is empty.");
        }

        return File(bytes, "text/css", "theme.css");
    }
}