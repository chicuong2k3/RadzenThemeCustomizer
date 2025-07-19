using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Options;
using Radzen;
using RadzenThemeCustomizer.Web;
using System.Net.Http.Json;
using TimeWarp.State;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Logging.SetMinimumLevel(LogLevel.None);

builder.Services.AddTimeWarpState();

var tempClient = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };

var settingsFile = builder.HostEnvironment.Environment == "Development"
    ? "appsettings.Development.json"
    : "appsettings.json";
var settings = await tempClient.GetFromJsonAsync<AppSettings>(settingsFile);
builder.Services.AddSingleton(settings!);

builder.Services.AddRadzenComponents();
builder.Services.AddScoped<ThemeManagerService>();
builder.Services.AddScoped<ColorVariableService>();

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(settings?.ApiBaseUrl ?? throw new ArgumentException("ApiBaseUrl not configured"))
});


await builder.Build().RunAsync();
