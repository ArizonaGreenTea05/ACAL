using CalendarView.Services.Music.Spotify;
using CalendarView.Web.Components;
using CalendarView.Web.Middleware;
using CalendarView.Web.Services;
using HelperProjects.AppsettingsEditor.Services;
using static CalendarView.Shared.Utils.Initialization;

namespace CalendarView.Web;

public class Program
{
    public static void Main(string[] args)
    {
        LoadAppsettings(out var calendars, out var design, out var loggingConfig, out var spotifyLoginData, out var authenticationConfig, out var editorConfig);

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.RegisterServices<FormFactor, SpotifyService>(calendars, design, spotifyLoginData, authenticationConfig);
        builder.Services.RegisterLogging(loggingConfig);

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        // Register EditorConfig and services
        builder.Services.AddSingleton(editorConfig);
        builder.Services.AddScoped<AppSettingsService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        // Add Basic Authentication middleware early in the pipeline
        app.UseMiddleware<BasicAuthenticationMiddleware>();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddAdditionalAssemblies(
                typeof(CalendarView.Shared._Imports).Assembly,
                typeof(HelperProjects.AppsettingsEditor.Components._Imports).Assembly);

        app.Run();
    }
}
