using CalendarView.Core.Models;
using CalendarView.Core.ViewModels;
using CalendarView.Services;
using CalendarView.Shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace CalendarView.Shared.Utils
{
    public static class Initialization
    {
        public static void LoadAppsettings(out Calendars calendars, out Design design, out LoggingConfig loggingConfig)
        {
            var appsettingsPaths = new[] { "../config/appsettings.json", "appsettings.json" };
            var path = appsettingsPaths.FirstOrDefault(File.Exists);
            if (path is null) throw new FileNotFoundException("appsettings not found");
            var appsettingsStream = File.OpenRead(path);
            var appsettingsBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonStream(appsettingsStream);
            var appsettings = appsettingsBuilder.Build();
            calendars = new Calendars();
            appsettings.GetSection(nameof(Calendars)).Bind(calendars);
            calendars.Definitions = calendars.Definitions.ToDictionary(kvp => kvp.Key.Replace('|', ':'), kvp => kvp.Value);

            design = new Design();
            appsettings.GetSection(nameof(Design)).Bind(design);

            loggingConfig = new LoggingConfig();
            appsettings.GetSection(nameof(LoggingConfig)).Bind(loggingConfig);
        }

        public static void RegisterServices(this IServiceCollection serviceCollection, Calendars calendars, Design design)
        {
            serviceCollection.AddSingleton(calendars);
            serviceCollection.AddSingleton(design);
            serviceCollection.AddHttpClient<CalendarService>();
            serviceCollection.AddTransient<CalendarViewModel>();
        }

        public static void RegisterLogging(this IServiceCollection serviceCollection, LoggingConfig loggingConfig)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Debug(outputTemplate:
                    "[{Level:u3}] [{SourceContext}] [{CallerMemberName}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(
                    formatter: new SingleLineTextFormatter(loggingConfig.LoggingTemplate),
                    path: loggingConfig.LoggingPath,
                    restrictedToMinimumLevel: LogEventLevel.Debug,
                    rollingInterval: RollingInterval.Day)
                .WriteTo.File(
                    formatter: new SingleLineTextFormatter(loggingConfig.LoggingTemplate),
                    loggingConfig.FilteredLoggingPath,
                    restrictedToMinimumLevel: LogEventLevel.Information,
                    rollingInterval: RollingInterval.Day)
                .CreateLogger();

            serviceCollection.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog(Log.Logger, true);
            });
        }
    }
}
