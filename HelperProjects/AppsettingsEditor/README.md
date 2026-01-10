# AppsettingsEditor

A user-friendly Blazor-based editor for CalendarView's appsettings.json file.

## Features

- **Upload/Download**: Upload existing appsettings.json files or download your configured settings
- **User-Friendly Interface**: Edit all configuration sections through intuitive forms instead of raw JSON
- **Validation**: Built-in validation ensures your configuration is valid
- **Sections Included**:
  - Authentication Configuration
  - Editor Access Control
  - Design Settings (colors, layouts, display options)
  - Calendar Definitions (URLs and customizations)
  - Spotify Integration
  - Logging Configuration

## Usage

### Standalone Mode

The editor can be run as a standalone application for creating or editing appsettings.json files:

```bash
cd HelperProjects/AppsettingsEditor
dotnet run
```

Then open your browser to `http://localhost:5000` (or the URL shown in the console).

### Integrated Mode (within CalendarView.Web)

The editor can be integrated into CalendarView.Web and accessed via a configurable path.

#### Enable the Editor

In `CalendarView.Web/appsettings.json`, add or update the `EditorConfig` section:

```json
{
  "EditorConfig": {
    "Enabled": true,
    "Path": "/editor"
  }
}
```

- `Enabled`: Set to `true` to enable the editor, `false` to disable it
- `Path`: The URL path where the editor will be accessible (default: `/editor`)

#### Access the Editor

Once enabled, navigate to the configured path in your browser:
- Default: `http://your-calendarview-url/editor`
- Custom: `http://your-calendarview-url/your-custom-path`

## Security Considerations

- The editor respects the authentication settings configured in appsettings.json
- When authentication is enabled in CalendarView.Web, the editor will also require authentication
- It's recommended to keep the editor disabled (`EditorConfig.Enabled = false`) in production environments unless necessary
- Consider enabling it only when needed for configuration changes

## Project Structure

- **Models/**: Data models representing the appsettings.json structure
- **Services/**: Business logic for loading, saving, and validating JSON
- **Components/Pages/**: Blazor components for the user interface
  - `Home.razor`: Main editor page with all configuration sections
  - `Error.razor`: Error handling page
- **wwwroot/**: Static assets (CSS, JavaScript for file downloads)

## Technical Details

- Built as a Razor Class Library for integration into CalendarView.Web
- Uses Newtonsoft.Json for JSON serialization
- Leverages existing model classes from CalendarView.Shared, CalendarView.Core, and CalendarView.Services
- Bootstrap 5 for responsive UI design
- Bootstrap Icons for visual enhancements
