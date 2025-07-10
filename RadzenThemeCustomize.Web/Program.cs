using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using RadzenThemeCustomizer.Web;
using System.Net.Http.Json;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var tempClient = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
var settings = await tempClient.GetFromJsonAsync<AppSettings>("appsettings.json");

builder.Services.AddRadzenComponents();
builder.Services.AddScoped<ThemeManagerService>();

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<LayoutEventService>();
builder.Services.AddScoped<ColorVariableService>();

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(settings?.ApiBaseUrl ?? throw new ArgumentException("ApiBaseUrl not configured"))
});

await builder.Build().RunAsync();
