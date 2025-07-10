using System.Net.Http.Json;
using System.Web;

namespace RadzenThemeCustomizer.Web;

public class ThemeManagerService
{
    private readonly HttpClient _httpClient;

    public ThemeManagerService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task UpdateThemeAsync(Dictionary<string, string> properties)
    {
        try
        {
            await _httpClient.PostAsJsonAsync("api/theme/update", properties);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[UpdateThemeAsync] Error: {ex.Message}");
        }
    }

    public async Task<string> GetThemeCssAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/theme");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[GetThemeCssAsync] Error: {ex.Message}");
        }

        return string.Empty;
    }

    public async Task<string> GetScssVariableAsync(string variableName)
    {
        try
        {
            var encoded = HttpUtility.UrlEncode(variableName);
            var response = await _httpClient.GetAsync($"api/theme/variable?name={encoded}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[GetScssVariableAsync] Error: {ex.Message}");
        }

        return string.Empty;
    }

    public async Task ResetThemeAsync(string themeName)
    {
        try
        {
            await _httpClient.PostAsJsonAsync("api/theme/reset", new { themeName });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ResetThemeAsync] Error: {ex.Message}");
        }
    }

    public async Task<byte[]> GetCssFileAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/theme/css-file");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsByteArrayAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[GetCssFileAsync] Error: {ex.Message}");
        }

        return Array.Empty<byte>();
    }
}
