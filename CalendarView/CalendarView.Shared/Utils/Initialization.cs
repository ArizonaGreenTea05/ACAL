using System.Runtime.InteropServices;
using CalendarView.Core.Models;
using CalendarView.Core.ViewModels;
using CalendarView.Services;
using CalendarView.Services.Music.Interfaces;
using CalendarView.Services.Music.Spotify;
using CalendarView.Services.Text;
using CalendarView.Shared.Models;
using CalendarView.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace CalendarView.Shared.Utils
{
    public static class Initialization
    {
        public static void LoadAppsettings(out Calendars calendars, out Design design, out LoggingConfig loggingConfig, out SpotifyServiceLoginData spotifyLoginData, out AuthenticationConfig authenticationConfig, out EditorConfig editorConfig)
        {
            var appsettingsPaths = new[] { "../config/appsettings.json", "appsettings.json" };
            var path = appsettingsPaths.FirstOrDefault(File.Exists) ?? throw new FileNotFoundException("appsettings not found");
            using var appsettingsStream = File.OpenRead(path);
            var appsettingsBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonStream(appsettingsStream)
                .AddEnvironmentVariables("ACAL");

            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                                  ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
            if (string.Equals(environmentName, "Development", StringComparison.OrdinalIgnoreCase))
            {
                appsettingsBuilder.AddJsonFile("appsettings.Development.json", optional: true);
            }

            var appsettings = appsettingsBuilder.Build();
            calendars = new Calendars();
            appsettings.GetSection(nameof(Calendars)).Bind(calendars);
            calendars.Definitions = calendars.Definitions.ToDictionary(kvp => kvp.Key.Replace('|', ':'), kvp => kvp.Value);

            design = new Design();
            appsettings.GetSection(nameof(Design)).Bind(design);

            loggingConfig = new LoggingConfig();
            appsettings.GetSection(nameof(LoggingConfig)).Bind(loggingConfig);

            spotifyLoginData = new SpotifyServiceLoginData();
            appsettings.GetSection(nameof(SpotifyServiceLoginData)).Bind(spotifyLoginData);

            authenticationConfig = new AuthenticationConfig();
            appsettings.GetSection(nameof(AuthenticationConfig)).Bind(authenticationConfig);

            editorConfig = new EditorConfig();
            appsettings.GetSection(nameof(EditorConfig)).Bind(editorConfig);
        }

        extension(IServiceCollection serviceCollection)
        {
            public void RegisterServices<TFormFactor, TMusicService>(Calendars calendars, Design design, IMusicServiceLoginData musicServiceLoginData, AuthenticationConfig authenticationConfig) where TFormFactor : class, IFormFactor where TMusicService : class, IMusicService
            {
                var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
                serviceCollection.AddSingleton<IFormFactor, TFormFactor>();
                serviceCollection.AddSingleton(calendars);
                serviceCollection.AddSingleton(design);
                serviceCollection.AddSingleton(musicServiceLoginData);
                serviceCollection.AddSingleton<IMusicService, TMusicService>();
                serviceCollection.AddKeyedSingleton("PictureRefreshInterval", new Common.TimeSpan(TimeSpan.FromMinutes(design.ChangePictureAfterMinutes)));
                serviceCollection.AddKeyedSingleton("PictureDirectory", design.PictureDirectory ?? string.Empty);
                serviceCollection.AddKeyedSingleton("TextRefreshInterval", new Common.TimeSpan(TimeSpan.FromMinutes(design.ChangeTextAfterMinutes)));
                serviceCollection.AddKeyedSingleton("TextDirectory", design.TextDirectory ?? string.Empty);
                serviceCollection.AddKeyedSingleton("AppdataFolderPath", isWindows
                    ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), nameof(CalendarView))
                    : new[]{ "../config", "" }.FirstOrDefault(Directory.Exists) ?? string.Empty);
                serviceCollection.AddSingleton<PictureService>();
                serviceCollection.AddSingleton<TextService>();
                serviceCollection.AddHttpClient<CalendarService>();
                serviceCollection.AddSingleton<CalendarViewModel>();
                serviceCollection.AddSingleton(authenticationConfig);
            }

            public void RegisterLogging(LoggingConfig loggingConfig)
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
}
