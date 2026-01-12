using AppsettingsEditor.Web.Components;
using CalendarView.Shared.Models;
using CalendarView.Shared.Utils;
using HelperProjects.AppsettingsEditor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add AppsettingsEditor services
builder.Services.AddScoped<AppSettingsService>();
builder.Services.RegisterLogging(new LoggingConfig
{
    LoggingTemplate = "| {Timestamp:HH:mm:ss:fff} | {Level:u3} | {SourceContext} | {CallerMemberName} | {Message:lj} | {CallerFilePath}:{CallerLineNumber} | {Exception} |",
    LoggingPath = "logs/log.debug",
    FilteredLoggingPath = "logs/log.information"
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(HelperProjects.AppsettingsEditor.Components.Pages.Home).Assembly);

app.Run();
