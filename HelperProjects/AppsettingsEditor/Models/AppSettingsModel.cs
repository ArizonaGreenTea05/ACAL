using CalendarView.Core.Models;
using CalendarView.Services.Music.Spotify;
using CalendarView.Shared.Models;
using Microsoft.Extensions.Logging;

namespace HelperProjects.AppsettingsEditor.Models;

public class AppSettingsModel
{
    public LoggingSection Logging { get; set; } = new();
    public string AllowedHosts { get; set; } = "*";
    public AuthenticationConfig AuthenticationConfig { get; set; } = new();
    public Design Design { get; set; } = new();
    public LoggingConfig LoggingConfig { get; set; } = new();
    public SpotifyServiceLoginData SpotifyServiceLoginData { get; set; } = new();
    public Calendars Calendars { get; set; } = new();
    public EditorConfig? EditorConfig { get; set; } = new();
}

public class LoggingSection
{
    public Dictionary<string, string> LogLevel { get; set; } = new()
    {
        { "Default", "Information" },
        { "Microsoft.AspNetCore", "Warning" }
    };
}

public class EditorConfig
{
    public bool Enabled { get; set; } = false;
    public string Path { get; set; } = "/editor";
}
