# API Reference

This document provides detailed reference information for ACAL's services, models, and interfaces.

## Table of Contents
- [Services](#services)
  - [CalendarService](#calendarservice)
  - [PictureService](#pictureservice)
  - [RefreshService](#refreshservice)
  - [SpotifyService](#spotifyservice)
- [Models](#models)
  - [Core Models](#core-models)
  - [Calendar Models](#calendar-models)
  - [Configuration Models](#configuration-models)
  - [Spotify Models](#spotify-models)
- [Interfaces](#interfaces)
- [Extensions](#extensions)

## Services

### CalendarService

**Namespace:** `CalendarView.Services`

**Purpose:** Loads and processes ICS calendar data from external sources.

#### Constructor

```csharp
public CalendarService(HttpClient httpClient, ILogger<CalendarService> logger)
```

**Parameters:**
- `httpClient` - HTTP client for fetching calendar data
- `logger` - Logger for diagnostic output

#### Methods

##### LoadEventsFromIcsAsync

Loads calendar events from an ICS URL.

```csharp
public async Task<List<CalendarEvent>?> LoadEventsFromIcsAsync(string icsUrl, int maxTries = 1)
```

**Parameters:**
- `icsUrl` (string) - URL of the ICS calendar file
- `maxTries` (int, optional) - Maximum number of retry attempts (default: 1)

**Returns:**
- `Task<List<CalendarEvent>?>` - List of calendar events, or null if loading fails

**Exceptions:**
- Logs errors but does not throw exceptions

**Example:**

```csharp
var events = await calendarService.LoadEventsFromIcsAsync(
    "https://calendar.example.com/events.ics",
    maxTries: 3
);

if (events != null)
{
    foreach (var evt in events)
    {
        Console.WriteLine($"{evt.Summary} at {evt.Start}");
    }
}
```

**Notes:**
- Uses Ical.Net library for parsing
- Automatically retries on failure based on `maxTries`
- Returns null on failure after all retries

---

### PictureService

**Namespace:** `CalendarView.Services`

**Purpose:** Manages random picture selection from a configured directory.

#### Constructor

```csharp
public PictureService(Design design, ILogger<PictureService> logger)
```

**Parameters:**
- `design` - Design configuration object
- `logger` - Logger for diagnostic output

#### Methods

##### GetRandomPicture

Retrieves a random picture from the configured directory.

```csharp
public async Task<string?> GetRandomPicture()
```

**Returns:**
- `Task<string?>` - Base64-encoded image data URL, or null if no images available

**Example:**

```csharp
var imageDataUrl = await pictureService.GetRandomPicture();

if (imageDataUrl != null)
{
    // Display in HTML: <img src="@imageDataUrl" />
}
```

**Notes:**
- Supports JPEG, PNG, and GIF formats
- Reads files from `Design.PictureDirectory`
- Returns data URL format: `data:image/jpeg;base64,...`

---

### RefreshService

**Namespace:** `CalendarView.Services`

**Purpose:** Manages periodic refresh of calendar and picture data.

#### Constructor

```csharp
public RefreshService(
    CalendarService calendarService,
    PictureService pictureService,
    Calendars calendars,
    Design design,
    ILogger<RefreshService> logger)
```

#### Methods

##### StartAsync

Starts the refresh service with periodic updates.

```csharp
public Task StartAsync(CancellationToken cancellationToken)
```

##### StopAsync

Stops the refresh service.

```csharp
public Task StopAsync(CancellationToken cancellationToken)
```

**Example:**

```csharp
// Typically started automatically by the framework
// Registered as: builder.Services.AddHostedService<RefreshService>();
```

**Notes:**
- Implements `IHostedService`
- Runs in background automatically
- Refresh intervals configured in `appsettings.json`

---

### SpotifyService

**Namespace:** `CalendarView.Services.Music.Spotify`

**Purpose:** Integrates with Spotify API for music playback control.

#### Constructor

```csharp
public SpotifyService(
    SpotifyLoginData loginData,
    ILogger<SpotifyService> logger)
```

#### Properties

```csharp
public bool IsAuthenticated { get; }
public CurrentlyPlaying? CurrentTrack { get; }
public bool IsPlaying { get; }
```

#### Methods

##### AuthenticateAsync

Authenticates with Spotify API.

```csharp
public async Task<bool> AuthenticateAsync()
```

**Returns:**
- `Task<bool>` - True if authentication successful

##### GetCurrentPlaybackAsync

Gets current playback information.

```csharp
public async Task<CurrentlyPlaying?> GetCurrentPlaybackAsync()
```

**Returns:**
- `Task<CurrentlyPlaying?>` - Current playback info, or null if nothing playing

##### PlayAsync

Resumes playback.

```csharp
public async Task<bool> PlayAsync()
```

##### PauseAsync

Pauses playback.

```csharp
public async Task<bool> PauseAsync()
```

##### SkipToNextAsync

Skips to next track.

```csharp
public async Task<bool> SkipToNextAsync()
```

##### SkipToPreviousAsync

Skips to previous track.

```csharp
public async Task<bool> SkipToPreviousAsync()
```

**Example:**

```csharp
if (await spotifyService.AuthenticateAsync())
{
    var currentTrack = await spotifyService.GetCurrentPlaybackAsync();
    
    if (currentTrack != null)
    {
        Console.WriteLine($"Now playing: {currentTrack.Item.Name}");
        
        if (spotifyService.IsPlaying)
        {
            await spotifyService.PauseAsync();
        }
        else
        {
            await spotifyService.PlayAsync();
        }
    }
}
```

---

## Models

### Core Models

#### CalendarEvent

**Namespace:** `CalendarView.Core.Models`

Base class for calendar events.

```csharp
public abstract class CalendarEvent
{
    public string Summary { get; set; }
    public string Description { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string Location { get; set; }
    public string CalendarName { get; set; }
    public string CalendarColor { get; set; }
}
```

**Properties:**
- `Summary` - Event title
- `Description` - Event description
- `Start` - Start date/time
- `End` - End date/time
- `Location` - Event location
- `CalendarName` - Name of calendar source
- `CalendarColor` - Hex color code for display

#### DefaultCalendarEvent

**Namespace:** `CalendarView.Core.Models`

Represents a standard timed event.

```csharp
public class DefaultCalendarEvent : CalendarEvent
{
    public TimeSpan Duration => End - Start;
}
```

**Additional Properties:**
- `Duration` - Calculated event duration

#### AllDayCalendarEvent

**Namespace:** `CalendarView.Core.Models`

Represents an all-day event.

```csharp
public class AllDayCalendarEvent : CalendarEvent
{
    public bool IsMultiDay => (End.Date - Start.Date).Days > 1;
}
```

**Additional Properties:**
- `IsMultiDay` - True if event spans multiple days

---

### Calendar Models

#### Calendar

**Namespace:** `CalendarView.Core.Models`

Represents calendar metadata and customization.

```csharp
public class Calendar
{
    public string Url { get; set; }
    public string CustomName { get; set; }
    public string Color { get; set; }
}
```

**Properties:**
- `Url` - ICS calendar URL (with pipe separator)
- `CustomName` - Display name for calendar
- `Color` - Hex color code

#### CalendarCustomization

**Namespace:** `CalendarView.Core.Models`

Customization options for individual calendars.

```csharp
public class CalendarCustomization
{
    public string Color { get; set; }
    public string CustomName { get; set; }
}
```

#### Calendars

**Namespace:** `CalendarView.Core.Models`

Collection of calendar configurations.

```csharp
public class Calendars
{
    public int RefreshAfterMinutes { get; set; }
    public Dictionary<string, CalendarCustomization> Definitions { get; set; }
}
```

**Properties:**
- `RefreshAfterMinutes` - How often to refresh calendar data
- `Definitions` - Dictionary of calendar URLs to customization options

---

### Configuration Models

#### Design

**Namespace:** `CalendarView.Shared.Models`

Application design configuration.

```csharp
public class Design
{
    // Language and Layout
    public string Language { get; set; }
    public string PageLayout { get; set; }
    
    // Colors
    public string BackColorName { get; set; }
    public string ForeColorName { get; set; }
    
    // Picture Settings
    public string PictureDirectory { get; set; }
    public double ChangePictureAfterMinutes { get; set; }
    
    // Display Options
    public bool ShowDate { get; set; }
    public bool ShowTime { get; set; }
    public bool ShowColorLegend { get; set; }
    public bool ShowScrollBar { get; set; }
    public double EventCardDimmingRatio { get; set; }
    
    // Swap Settings
    public bool SwapPictureAndContentInPortrait { get; set; }
    public bool SwapPictureAndContentInLandscape { get; set; }
    
    // Format Strings
    public string LongDateFormat { get; set; }
    public string ShortDateFormat { get; set; }
    public string ShortTimeFormat { get; set; }
    public string LongMonthFormat { get; set; }
    public string LongDayFormat { get; set; }
    
    // Layout-Specific Designs
    public Dictionary<string, LayoutDesign> Designs { get; set; }
}
```

#### LoggingConfig

**Namespace:** `CalendarView.Shared.Models`

Logging configuration.

```csharp
public class LoggingConfig
{
    public string LoggingTemplate { get; set; }
    public string LoggingPath { get; set; }
    public string FilteredLoggingPath { get; set; }
}
```

#### AuthenticationConfig

**Namespace:** (Loaded in initialization)

Authentication configuration.

```csharp
public class AuthenticationConfig
{
    public bool Enabled { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}
```

---

### Spotify Models

#### SpotifyLoginData

**Namespace:** `Spotify.Models`

Spotify authentication credentials.

```csharp
public class SpotifyLoginData
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public SpotifyToken AuthToken { get; set; }
}
```

#### SpotifyToken

**Namespace:** `Spotify.Models`

Spotify OAuth token information.

```csharp
public class SpotifyToken
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public string TokenType { get; set; }
    public int ExpiresIn { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public bool IsExpired => DateTime.UtcNow >= CreatedAt.AddSeconds(ExpiresIn);
}
```

---

### View Models

#### Notification

**Namespace:** `CalendarView.Core.Models`

User notification model.

```csharp
public class Notification
{
    public string Message { get; set; }
    public NotificationKind Kind { get; set; }
    public DateTime Timestamp { get; set; }
}
```

**Enums:**

```csharp
public enum NotificationKind
{
    Information,
    Warning,
    Error
}
```

---

## Interfaces

### IFormFactor

**Namespace:** `CalendarView.Shared.Services`

**Purpose:** Device form factor detection.

```csharp
public interface IFormFactor
{
    FormFactorType GetFormFactor();
}
```

**Form Factor Types:**

```csharp
public enum FormFactorType
{
    Desktop,
    Tablet,
    Mobile
}
```

**Implementation Example:**

```csharp
public class FormFactor : IFormFactor
{
    public FormFactorType GetFormFactor()
    {
        // Detection logic based on user agent or screen size
        return FormFactorType.Desktop;
    }
}
```

### IMusicService

**Namespace:** `CalendarView.Services.Music.Interfaces`

**Purpose:** Abstract interface for music services.

```csharp
public interface IMusicService
{
    Task<bool> AuthenticateAsync();
    Task<bool> PlayAsync();
    Task<bool> PauseAsync();
    Task<bool> SkipToNextAsync();
    Task<bool> SkipToPreviousAsync();
}
```

**Purpose:** Allows for multiple music service implementations (Spotify, Apple Music, etc.)

---

## Extensions

### ColorExtensions

**Namespace:** `Common.UI.Extensions`

**Purpose:** Color manipulation utilities.

```csharp
public static class ColorExtensions
{
    public static string ToHexString(this Color color)
    {
        return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
    }
    
    public static Color WithAlpha(this Color color, byte alpha)
    {
        return Color.FromArgb(alpha, color.R, color.G, color.B);
    }
}
```

**Example:**

```csharp
var color = Color.Red;
var hex = color.ToHexString(); // "#FF0000"
var transparent = color.WithAlpha(128); // 50% transparent red
```

### DateTimeExtensions

**Namespace:** `Common.UI.Extensions`

**Purpose:** Date/time utility methods.

```csharp
public static class DateTimeExtensions
{
    public static bool IsToday(this DateTime dateTime)
    {
        return dateTime.Date == DateTime.Today;
    }
    
    public static bool IsTomorrow(this DateTime dateTime)
    {
        return dateTime.Date == DateTime.Today.AddDays(1);
    }
    
    public static string ToRelativeString(this DateTime dateTime)
    {
        if (dateTime.IsToday())
            return "Today";
        if (dateTime.IsTomorrow())
            return "Tomorrow";
        return dateTime.ToString("dddd, MMMM d");
    }
}
```

**Example:**

```csharp
var now = DateTime.Now;
var isToday = now.IsToday(); // true

var tomorrow = DateTime.Now.AddDays(1);
var relative = tomorrow.ToRelativeString(); // "Tomorrow"
```

---

## Usage Examples

### Loading and Displaying Calendar Events

```csharp
@inject CalendarService CalendarService

@code {
    private List<CalendarEvent> events = [];
    
    protected override async Task OnInitializedAsync()
    {
        var calendars = new Dictionary<string, CalendarCustomization>
        {
            ["https|//example.com/cal.ics"] = new()
            {
                Color = "#FF0000",
                CustomName = "Personal"
            }
        };
        
        foreach (var (url, customization) in calendars)
        {
            var calendarEvents = await CalendarService.LoadEventsFromIcsAsync(
                url.Replace("|", ":")
            );
            
            if (calendarEvents != null)
            {
                foreach (var evt in calendarEvents)
                {
                    evt.CalendarColor = customization.Color;
                    evt.CalendarName = customization.CustomName;
                }
                
                events.AddRange(calendarEvents);
            }
        }
        
        // Sort by start time
        events = events.OrderBy(e => e.Start).ToList();
    }
}
```

### Implementing a Custom Music Service

```csharp
public class CustomMusicService : IMusicService
{
    public async Task<bool> AuthenticateAsync()
    {
        // Implement authentication
        return true;
    }
    
    public async Task<bool> PlayAsync()
    {
        // Implement play
        return true;
    }
    
    public async Task<bool> PauseAsync()
    {
        // Implement pause
        return true;
    }
    
    public async Task<bool> SkipToNextAsync()
    {
        // Implement skip next
        return true;
    }
    
    public async Task<bool> SkipToPreviousAsync()
    {
        // Implement skip previous
        return true;
    }
}

// Register in Program.cs
builder.Services.AddSingleton<IMusicService, CustomMusicService>();
```

### Creating a Custom Component

```razor
@* CustomEventCard.razor *@
@namespace CalendarView.Shared.Components

<div class="custom-event-card" style="border-left: 4px solid @Event.CalendarColor">
    <div class="event-time">
        @Event.Start.ToString("HH:mm") - @Event.End.ToString("HH:mm")
    </div>
    <div class="event-title">@Event.Summary</div>
    @if (!string.IsNullOrEmpty(Event.Location))
    {
        <div class="event-location">📍 @Event.Location</div>
    }
</div>

@code {
    [Parameter, EditorRequired]
    public CalendarEvent Event { get; set; } = null!;
}
```

---

## Best Practices

### Service Usage

- **Always use dependency injection** for services
- **Handle null returns** from async methods
- **Log errors** but don't throw exceptions in UI code
- **Use cancellation tokens** for long-running operations

### Model Usage

- **Validate data** before creating models
- **Use appropriate model types** (AllDayCalendarEvent vs DefaultCalendarEvent)
- **Don't mutate models** in multiple places
- **Consider immutability** for thread-safe operations

### Extension Methods

- **Keep focused** - one responsibility per extension
- **Document behavior** clearly
- **Consider null inputs** - handle gracefully
- **Use descriptive names** - make intent clear

---

## Version Compatibility

This API reference is valid for:
- **ACAL Version:** 1.x
- **.NET Version:** 10.0+
- **Breaking Changes:** None expected in 1.x releases

For version-specific changes, see the [Changelog](../CHANGELOG.md).
