# AppsettingsEditor.Web

Standalone web application wrapper for the AppsettingsEditor Razor Class Library.

## Purpose

This project allows you to run the AppsettingsEditor as a standalone web application for creating or editing CalendarView's appsettings.json files offline, without needing to integrate it into CalendarView.Web.

## Running the Standalone Editor

```bash
cd HelperProjects/AppsettingsEditor.Web
dotnet run
```

Then open your browser to `http://localhost:5000` (or the URL shown in the console).

## Features

All features from the AppsettingsEditor library are available:
- Upload/download appsettings.json files
- User-friendly form-based editing
- Spotify token generation
- Full configuration management

## When to Use

Use this standalone version when you need to:
- Create or edit appsettings.json files offline
- Work on configuration without running the full CalendarView application
- Generate Spotify authentication tokens
- Prepare configuration files for deployment

For integrated usage within CalendarView.Web, refer to the main AppsettingsEditor README.
