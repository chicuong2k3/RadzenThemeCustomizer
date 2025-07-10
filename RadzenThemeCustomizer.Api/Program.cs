using JavaScriptEngineSwitcher.Core;
using JavaScriptEngineSwitcher.V8;
using RadzenThemeCustomizer.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

JsEngineSwitcher.Current.EngineFactories.AddV8();
JsEngineSwitcher.Current.DefaultEngineName = V8JsEngine.EngineName;
builder.Services.AddScoped<ThemeManagerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseCors();

app.MapControllers();

app.Run();
