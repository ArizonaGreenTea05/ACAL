using CalendarView.Services.Music.Models;
using CalendarView.Services.Music.Spotify;
using CalendarView.Web.Components;
using CalendarView.Web.Middleware;
using CalendarView.Web.Services;
using HelperProjects.AppsettingsEditor.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using static CalendarView.Shared.Utils.Initialization;

namespace CalendarView.Web;

public class Program
{
    private const int AuthenticationCookieExpirationDays = 7;

    public static void Main(string[] args)
    {
        LoadAppsettings(out var calendars, out var design, out var loggingConfig, out var loginDataCollection, out var authenticationConfig, out var editorConfig);

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.RegisterLogging(loggingConfig);
        builder.Services.RegisterServices<FormFactor>(calendars, design, loginDataCollection, authenticationConfig);

        // Add cookie authentication
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.Name = "ACAL.Auth";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(AuthenticationCookieExpirationDays);
                options.LoginPath = "/";
                options.AccessDeniedPath = "/";
            });
        builder.Services.AddAuthorization();

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

        // Add authentication and authorization
        app.UseAuthentication();
        app.UseAuthorization();

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
