using Blazored.LocalStorage;
using RadzenThemeCustomizer.Shared;
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

    public async Task<ThemeDto?> GetSingleTheme()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<ThemeDto>("api/theme/single");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[CreateThemeAsync] Error: {ex.Message}");
            return null;
        }
    }

    public async Task<ThemeDto?> CreateThemeAsync(CreateThemeRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/theme", request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ThemeDto>();
            }
            else
            {
                Console.WriteLine($"[CreateThemeAsync] Failed to create theme. Status code: {response.StatusCode}");
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[CreateThemeAsync] Error: {ex.Message}");
            return null;
        }
    }

    public async Task UpdateThemeAsync(UpdateThemeRequest request)
    {
        try
        {
            await _httpClient.PostAsJsonAsync("api/theme/update", request);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[UpdateThemeAsync] Error: {ex.Message}");
        }
    }

    public async Task<string> GetThemeCssAsync(GetThemeCssRequest request)
    {
        try
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["themeName"] = request.ThemeName;

            var response = await _httpClient.GetAsync($"api/theme?{query}");
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

    public async Task<string> GetScssVariableAsync(GetVariableRequest request)
    {
        try
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["variableName"] = request.VariableName;
            query["themeName"] = request.ThemeName;
            var response = await _httpClient.GetAsync($"api/theme/variable?{query}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[GetScssVariableAsync] Error: {ex.Message}");
            return string.Empty;
        }

        return string.Empty;
    }

    public async Task<byte[]> DownloadCssFileAsync(DownloadCssFileRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/theme/css-file", request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsByteArrayAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DownloadCssFileAsync] Error: {ex.Message}");
        }

        return Array.Empty<byte>();
    }
}
