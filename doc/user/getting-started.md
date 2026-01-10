# Getting Started with ACAL

Welcome to ACAL (ACAL Calendar And Layout)! This guide will help you install and run ACAL on your system.

## Table of Contents
- [Prerequisites](#prerequisites)
- [Installation Methods](#installation-methods)
  - [Docker Installation (Recommended)](#docker-installation-recommended)
  - [Manual Installation](#manual-installation)
- [First Run](#first-run)
- [Next Steps](#next-steps)

## Prerequisites

Before installing ACAL, ensure you have one of the following:

### For Docker Installation
- **[Docker Desktop](https://www.docker.com/products/docker-desktop/)** (version 20.10 or later)
- A text editor for configuration files

### For Manual Installation
- **[.NET SDK](https://dotnet.microsoft.com/download)** (version 10.0 or later)
- **[Git](https://git-scm.com/downloads)** (for cloning the repository)
- A text editor for configuration files

## Installation Methods

### Docker Installation (Recommended)

Docker is the easiest way to run ACAL. The official Docker image is automatically updated with each release.

#### Step 1: Pull the Docker Image

```bash
docker pull arizonagreentea0905/acal:latest
```

#### Step 2: Create Configuration Directory

Create a directory for your configuration files:

```bash
mkdir -p ~/acal-config
```

#### Step 3: Create Configuration File

Create a file named `appsettings.json` in your configuration directory. See the [Configuration Guide](configuration.md) for detailed options. Here's a minimal example:

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
    "Enabled": false,
    "Username": "",
    "Password": ""
  },
  "Design": {
    "PageLayout": "AgendaWithImageAndBackground",
    "LongDateFormat": "dddd, dd. MMMM yyyy",
    "ShortDateFormat": "dd.MM.",
    "ShortTimeFormat": "HH:mm",
    "LongMonthFormat": "MMMM",
    "LongDayFormat": "dddd"
  },
  "Calendars": {
    "RefreshAfterMinutes": 60,
    "Definitions": {
      "https|//ics.tools/Ferien/nordrhein-westfalen.ics": {
        "Color": "#FFFD00",
        "CustomName": "Holidays"
      }
    }
  }
}
```

**Important:** In calendar URLs, replace `://` with `|//` (pipe instead of colon) to avoid JSON deserialization issues.

#### Step 4: Run the Container

Run ACAL with the following command:

```bash
docker run -d \
  --name acal \
  -p 5000:8080 \
  -v ~/acal-config:/app/config \
  -v ~/acal-images:/app/images \
  arizonagreentea0905/acal:latest
```

**Volume Mappings:**
- `~/acal-config:/app/config` - Configuration directory (required)
- `~/acal-images:/app/images` - Image directory for photo display (optional)

#### Step 5: Access ACAL

Open your web browser and navigate to:
```
http://localhost:5000
```

### Manual Installation

If you prefer to build and run ACAL from source:

#### Step 1: Clone the Repository

```bash
git clone https://github.com/ArizonaGreenTea05/ACAL.git
cd ACAL
```

#### Step 2: Restore Dependencies

```bash
dotnet restore
```

#### Step 3: Configure the Application

Edit the `appsettings.json` file in the `CalendarView/CalendarView.Web` directory. See the [Configuration Guide](configuration.md) for detailed options.

#### Step 4: Build the Application

```bash
dotnet build
```

#### Step 5: Run the Application

```bash
dotnet run --project CalendarView/CalendarView.Web
```

The application will start and display the URLs it's listening on (typically `http://localhost:5000` and `https://localhost:5001`).

## First Run

When you first access ACAL:

1. **Authentication (if enabled):** You'll be prompted for credentials if you've enabled authentication in your configuration.

2. **Default View:** The home page will display based on your configured `PageLayout` setting.

3. **Calendar Data:** If you've configured calendar sources, ACAL will begin loading events. This may take a moment on first run.

4. **Photos (if configured):** If you've specified a `PictureDirectory`, ACAL will cycle through images from that directory.

## Next Steps

Now that ACAL is running, you can:

- **Customize your setup:** Read the [Configuration Guide](configuration.md) to learn about all available options
- **Explore the interface:** Check out the [User Interface Guide](user-interface.md) to learn about different layouts and features
- **Add calendars:** Configure multiple calendar sources to aggregate all your events
- **Integrate Spotify:** Set up Spotify integration to control music playback
- **Troubleshoot issues:** Visit the [Troubleshooting Guide](troubleshooting.md) if you encounter any problems

## Quick Tips

- 💡 Use the Docker installation for easier updates and deployment
- 🔒 Enable authentication if ACAL will be accessible from outside your local network
- 📅 You can add multiple calendar sources by adding more entries to the `Calendars.Definitions` section
- 🖼️ Supported image formats include JPEG, PNG, and GIF
- 🔄 Calendar data refreshes automatically based on your `RefreshAfterMinutes` setting
