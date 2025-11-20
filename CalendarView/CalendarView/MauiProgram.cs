using CalendarView.Services;
using Microsoft.Extensions.Logging;
using static CalendarView.Shared.Utils.Initialization;

namespace CalendarView
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            LoadAppsettings(out var calendars, out var design, out var loggingConfig);

            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.RegisterServices<FormFactor>(calendars, design);
            builder.Services.RegisterLogging(loggingConfig);

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
