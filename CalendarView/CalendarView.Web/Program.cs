using CalendarView.Web.Components;
using CalendarView.Web.Services;
using static CalendarView.Shared.Utils.Initialization;

namespace CalendarView;

public class Program
{
    public static void Main(string[] args)
    {
        LoadAppsettings(out var calendars, out var design, out var loggingConfig);

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.RegisterServices<FormFactor>(calendars, design);
        builder.Services.RegisterLogging(loggingConfig);

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddAdditionalAssemblies(typeof(CalendarView.Shared._Imports).Assembly);

        app.Run();
    }
}
