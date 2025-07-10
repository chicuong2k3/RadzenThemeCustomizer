using Microsoft.AspNetCore.Mvc;
using RadzenThemeCustomizer.Api;

namespace RadzenThemeCustomizer.Api.Controllers;

[Route("download")]
[ApiController]
public class DownloadController : ControllerBase
{
    private readonly ThemeManagerService _themeManagerService;

    public DownloadController(ThemeManagerService themeManagerService)
    {
        _themeManagerService = themeManagerService;
    }

    [HttpGet("css")]
    public async Task<IActionResult> DownloadCss()
    {
        var cssFile = await _themeManagerService.GetCssFileAsync();
        if (cssFile == null || cssFile.Length == 0)
        {
            return NotFound("CSS file not found or empty.");
        }

        return File(cssFile, "text/css", "radzen-theme.css");
    }
}
