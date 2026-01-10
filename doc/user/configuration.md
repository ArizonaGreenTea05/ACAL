# Configuration Guide

This guide explains all configuration options available in ACAL through the `appsettings.json` file.

## Table of Contents
- [Configuration File Location](#configuration-file-location)
- [Basic Configuration](#basic-configuration)
- [Authentication](#authentication)
- [Design and Appearance](#design-and-appearance)
- [Calendar Configuration](#calendar-configuration)
- [Spotify Integration](#spotify-integration)
- [Logging Configuration](#logging-configuration)
- [Complete Example](#complete-example)

## Configuration File Location

### Docker Deployment
When running ACAL in Docker, the configuration file should be placed in the directory mapped to `/app/config`:
```bash
~/acal-config/appsettings.json
```

### Manual Deployment
For manual installations, edit the configuration file at:
```bash
CalendarView/CalendarView.Web/appsettings.json
```

## Basic Configuration

### Logging Levels

Control the verbosity of application logs:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

**Log Levels:**
- `Trace` - Most detailed logs
- `Debug` - Debugging information
- `Information` - General informational messages (recommended)
- `Warning` - Warning messages
- `Error` - Error messages
- `Critical` - Critical failures

### Allowed Hosts

Define which hosts can access the application:

```json
{
  "AllowedHosts": "*"
}
```

Use `"*"` to allow all hosts, or specify specific domains: `"localhost;example.com"`

## Authentication

ACAL supports HTTP Basic Authentication to protect access.

### Configuration

```json
{
  "AuthenticationConfig": {
    "Enabled": true,
    "Username": "your-username",
    "Password": "your-secure-password"
  }
}
```

**Options:**
- `Enabled` (boolean) - Enable or disable authentication
- `Username` (string) - Username for authentication
- `Password` (string) - Password for authentication

### Security Best Practices

- ✅ Use strong, unique passwords
- ✅ Enable HTTPS in production environments
- ✅ Change default credentials immediately
- ✅ Use environment variables or secrets management for sensitive data

### Kiosk Mode

For kiosk devices, you can embed credentials in the URL:
```
http://username:password@your-server.com
```

⚠️ **Warning:** Only use this method on trusted, local networks.

## Design and Appearance

Customize the visual appearance and behavior of ACAL.

### Basic Design Settings

```json
{
  "Design": {
    "Language": "en",
    "PageLayout": "AgendaWithImageAndBackground",
    "BackColorName": "#1c1c1c",
    "ForeColorName": "LightGray"
  }
}
```

**Options:**
- `Language` - Interface language (e.g., "en", "de")
- `PageLayout` - Default layout to display (see [Available Layouts](#available-layouts))
- `BackColorName` - Background color (hex code or named color)
- `ForeColorName` - Text/foreground color (hex code or named color)

### Available Layouts

- `AgendaWithImageAndBackground` - Agenda view with side image and background
- `AgendaWithBackground` - Agenda view with background only
- `CalendarWithImageAndBackground` - Calendar view with side image and background
- `CalendarWithBackground` - Calendar view with background only

### Picture Configuration

Display photos from a specified directory:

```json
{
  "Design": {
    "PictureDirectory": "../images",
    "ChangePictureAfterMinutes": 0.2,
    "SwapPictureAndContentInPortrait": false,
    "SwapPictureAndContentInLandscape": false
  }
}
```

**Options:**
- `PictureDirectory` - Path to directory containing images
- `ChangePictureAfterMinutes` - How often to change the displayed image (in minutes)
- `SwapPictureAndContentInPortrait` - Switch image position in portrait mode
- `SwapPictureAndContentInLandscape` - Switch image position in landscape mode

### Display Options

```json
{
  "Design": {
    "ShowDate": true,
    "ShowTime": true,
    "ShowColorLegend": true,
    "ShowScrollBar": false,
    "EventCardDimmingRatio": 0.3
  }
}
```

**Options:**
- `ShowDate` - Display current date
- `ShowTime` - Display current time
- `ShowColorLegend` - Show calendar color legend
- `ShowScrollBar` - Show scrollbar (useful for long event lists)
- `EventCardDimmingRatio` - Dimming ratio of event cards signature color (0.0 to 1.0)

### Date and Time Formatting

Customize how dates and times are displayed:

```json
{
  "Design": {
    "LongDateFormat": "dddd, dd. MMMM yyyy",
    "ShortDateFormat": "dd.MM.",
    "ShortTimeFormat": "HH:mm",
    "LongMonthFormat": "MMMM",
    "LongDayFormat": "dddd"
  }
}
```

**Format Specifiers:**
- `dd` - Day of month (01-31)
- `MM` - Month (01-12)
- `yyyy` - Four-digit year
- `HH` - Hour in 24-hour format (00-23)
- `mm` - Minutes (00-59)
- `dddd` - Full day name (e.g., "Monday")
- `MMMM` - Full month name (e.g., "January")

### Layout-Specific Design

Override design settings for specific layouts:

```json
{
  "Design": {
    "Designs": {
      "AgendaWithBackground": {
        "CustomBackgroundImageBlur": "2px"
      },
      "CalendarWithBackground": {
        "CustomBackgroundImageBlur": "5px"
      }
    }
  }
}
```

## Calendar Configuration

Configure calendar sources and refresh behavior.

### Basic Calendar Settings

```json
{
  "Calendars": {
    "RefreshAfterMinutes": 60,
    "Definitions": {}
  }
}
```

**Options:**
- `RefreshAfterMinutes` - How often to refresh calendar data (in minutes)

### Adding Calendar Sources

Add multiple calendar sources with custom names and colors:

```json
{
  "Calendars": {
    "Definitions": {
      "https|//www.example.com/calendar.ics": {
        "Color": "#FF0000",
        "CustomName": "Personal"
      },
      "https|//www.example.com/work.ics": {
        "Color": "#00FF00",
        "CustomName": "Work"
      },
      "https|//www.example.com/birthdays.ics": {
        "Color": "#0000FF",
        "CustomName": "Birthdays"
      }
    }
  }
}
```

**Important:** Replace `://` with `|//` in URLs (use pipe instead of colon) to avoid JSON parsing issues.

**Options:**
- `Color` - Hex color code for calendar events (e.g., "#FF0000" for red)
- `CustomName` - Display name for the calendar

### Supported Calendar Sources

ACAL supports any ICS (iCalendar) format calendar, including:
- ✅ Google Calendar (export link)
- ✅ Microsoft Outlook/Office 365
- ✅ Apple iCloud Calendar
- ✅ CalDAV calendars
- ✅ Public holiday calendars
- ✅ Any other ICS-compatible calendar service

## Spotify Integration

Integrate Spotify for music playback control.

### Configuration

```json
{
  "SpotifyServiceLoginData": {
    "ClientId": "your-client-id",
    "ClientSecret": "your-client-secret",
    "AuthToken": {
      "AccessToken": "your-access-token",
      "RefreshToken": "your-refresh-token",
      "TokenType": "Bearer",
      "ExpiresIn": 3600,
      "CreatedAt": "2025-12-12T13:35:45.5304927Z"
    }
  }
}
```

### Setup Steps

1. **Create Spotify Application:**
   - Go to [Spotify Developer Dashboard](https://developer.spotify.com/dashboard)
   - Create a new application
   - Note your Client ID and Client Secret

2. **Generate Auth Token:**
   - Use the SpotifyTokenHelper included in the HelperProjects folder
   - Or use the helper attached to releases
   - Configure the helper with your Client ID and Client Secret
   - Run the helper to generate access and refresh tokens

3. **Update Configuration:**
   - Add the Client ID, Client Secret, and token information to your `appsettings.json`

### Requirements

- Active Spotify Premium account
- Spotify application credentials from the Developer Dashboard

## Logging Configuration

Configure detailed application logging.

```json
{
  "LoggingConfig": {
    "LoggingTemplate": "| {Timestamp:HH:mm:ss:fff} | {Level:u3} | {SourceContext} | {CallerMemberName} | {Message:lj} | {CallerFilePath}:{CallerLineNumber} | {Exception} |",
    "LoggingPath": "logs/log.debug",
    "FilteredLoggingPath": "logs/log.information"
  }
}
```

**Options:**
- `LoggingTemplate` - Format template for log entries
- `LoggingPath` - Path for debug-level logs
- `FilteredLoggingPath` - Path for filtered (information-level and above) logs

## Complete Example

Here's a complete, production-ready configuration:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "AuthenticationConfig": {
    "Enabled": true,
    "Username": "admin",
    "Password": "your-secure-password-here"
  },
  "Design": {
    "Language": "en",
    "PictureDirectory": "../images",
    "ChangePictureAfterMinutes": 5,
    "BackColorName": "#1c1c1c",
    "ForeColorName": "LightGray",
    "EventCardDimmingRatio": 0.3,
    "SwapPictureAndContentInPortrait": false,
    "SwapPictureAndContentInLandscape": false,
    "ShowDate": true,
    "ShowTime": true,
    "ShowColorLegend": true,
    "ShowScrollBar": false,
    "PageLayout": "AgendaWithImageAndBackground",
    "LongDateFormat": "dddd, dd. MMMM yyyy",
    "ShortDateFormat": "dd.MM.",
    "ShortTimeFormat": "HH:mm",
    "LongMonthFormat": "MMMM",
    "LongDayFormat": "dddd",
    "Designs": {
      "AgendaWithBackground": {
        "CustomBackgroundImageBlur": "2px"
      },
      "CalendarWithBackground": {
        "CustomBackgroundImageBlur": "2px"
      }
    }
  },
  "LoggingConfig": {
    "LoggingTemplate": "| {Timestamp:HH:mm:ss:fff} | {Level:u3} | {SourceContext} | {CallerMemberName} | {Message:lj} | {CallerFilePath}:{CallerLineNumber} | {Exception} |",
    "LoggingPath": "logs/log.debug",
    "FilteredLoggingPath": "logs/log.information"
  },
  "SpotifyServiceLoginData": {
    "ClientId": "your-spotify-client-id",
    "ClientSecret": "your-spotify-client-secret",
    "AuthToken": {
      "AccessToken": "your-access-token",
      "RefreshToken": "your-refresh-token",
      "TokenType": "Bearer",
      "ExpiresIn": 3600,
      "CreatedAt": "2025-12-12T13:35:45.5304927Z"
    }
  },
  "Calendars": {
    "RefreshAfterMinutes": 60,
    "Definitions": {
      "https|//calendar.google.com/calendar/ical/example/basic.ics": {
        "Color": "#FF0000",
        "CustomName": "Personal"
      },
      "https|//outlook.office365.com/owa/calendar/example/calendar.ics": {
        "Color": "#00FF00",
        "CustomName": "Work"
      },
      "https|//ics.tools/Ferien/nordrhein-westfalen.ics": {
        "Color": "#FFFD00",
        "CustomName": "Holidays"
      }
    }
  }
}
```

## Configuration Tips

- 💡 Start with minimal configuration and add features gradually
- 🔄 Changes to most settings require restarting the application
- 📝 Keep a backup of your working configuration
- 🔒 Store sensitive data (passwords, API keys) securely
- 🐳 For Docker, use environment variables for sensitive values
- 📊 Monitor logs to troubleshoot configuration issues
