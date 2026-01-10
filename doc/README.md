# ACAL Documentation

Welcome to the ACAL (ACAL Calendar And Layout) documentation! This directory contains comprehensive guides for both users and developers.

## Documentation Structure

```
doc/
├── user/                    # User documentation
│   ├── getting-started.md   # Installation and setup guide
│   ├── configuration.md     # Configuration options reference
│   ├── user-interface.md    # UI features and layouts guide
│   └── troubleshooting.md   # Common issues and solutions
│
├── developer/               # Developer documentation
│   ├── architecture.md      # Architecture overview
│   ├── development-guide.md # Development setup and workflow
│   ├── api-reference.md     # API documentation
│   └── deployment.md        # Production deployment guide
│
└── images/                  # Screenshots and images
```

## Quick Links

### For Users

| Document | Description |
|----------|-------------|
| [Getting Started](user/getting-started.md) | Installation instructions and first-time setup |
| [Configuration Guide](user/configuration.md) | Complete configuration reference |
| [User Interface Guide](user/user-interface.md) | Learn about layouts and features |
| [Troubleshooting](user/troubleshooting.md) | Solutions to common problems |

### For Developers

| Document | Description |
|----------|-------------|
| [Architecture](developer/architecture.md) | System architecture and design |
| [Development Guide](developer/development-guide.md) | Setup and contribution workflow |
| [API Reference](developer/api-reference.md) | Services, models, and interfaces |
| [Deployment Guide](developer/deployment.md) | Production deployment instructions |

## Getting Started

### I'm a New User

1. Start with [Getting Started](user/getting-started.md) to install ACAL
2. Configure your setup using the [Configuration Guide](user/configuration.md)
3. Explore features in the [User Interface Guide](user/user-interface.md)
4. If you run into issues, check [Troubleshooting](user/troubleshooting.md)

### I'm a Developer

1. Read the [Architecture](developer/architecture.md) to understand the system
2. Follow the [Development Guide](developer/development-guide.md) to set up your environment
3. Reference the [API Documentation](developer/api-reference.md) while coding
4. Use the [Deployment Guide](developer/deployment.md) for production deployment

### I Want to Contribute

1. Read the [Development Guide](developer/development-guide.md) - Contributing section
2. Check the [GitHub Issues](https://github.com/ArizonaGreenTea05/ACAL/issues)
3. Follow the code style guidelines in the development guide
4. Submit pull requests with clear descriptions

## What is ACAL?

ACAL (ACAL Calendar And Layout) is a versatile and highly customizable web application designed to help users stay organized and enjoy their digital content. Built with Blazor Server and .NET 10, ACAL provides:

- 📅 **Interactive Calendar** - Aggregate multiple calendar sources
- 🖼️ **Photo Display** - Showcase personal photos
- 🎵 **Spotify Integration** - Control music playback
- 🎨 **Customizable Layouts** - Multiple view options
- 🔒 **Authentication** - Optional password protection
- 🐳 **Docker Support** - Easy deployment

## Key Features

### Calendar Management
- Support for multiple ICS calendar sources
- Google Calendar, Outlook, iCloud compatible
- Color-coded events by calendar
- All-day event support
- Automatic refresh

### Display Options
- **Agenda Views** - List upcoming events chronologically
- **Calendar Views** - Traditional month calendar
- **Photo Integration** - Display alongside calendar
- **Responsive Design** - Adapts to any screen size

### Music Integration
- Spotify playback control
- Display current track information
- Play/pause, skip controls
- Requires Spotify Premium

### Customization
- Multiple layout options
- Configurable colors and themes
- Date/time format customization
- Language support

## Common Use Cases

### Home Display
Mount a tablet or screen to display your family calendar, photos, and control music throughout your home.

### Office Dashboard
Display team calendars and important dates on an office monitor or kiosk.

### Personal Information Center
Keep track of multiple calendars (work, personal, family) in one unified view.

### Digital Photo Frame
Combine calendar functionality with a rotating photo slideshow.

## System Requirements

### For Users (Docker)
- Docker Desktop 20.10+
- 100MB disk space
- Internet connection

### For Users (Manual)
- .NET 10 Runtime
- 100MB disk space
- Internet connection

### For Developers
- .NET SDK 10.0+
- Visual Studio 2022 or VS Code
- Git
- Docker (optional)

## Support

### Documentation Issues
If you find errors or missing information in this documentation, please:
1. Open an issue on [GitHub Issues](https://github.com/ArizonaGreenTea05/ACAL/issues)
2. Label it as "documentation"
3. Describe the problem or suggest improvements

### Application Issues
For bugs or feature requests:
- **Community:** [GitHub Issues](https://github.com/ArizonaGreenTea05/ACAL/issues)
- **Developers:** [YouTrack](https://sugoi.youtrack.cloud/projects/ACAL/agiles/195-1/current)

### Getting Help
1. Check the [Troubleshooting Guide](user/troubleshooting.md)
2. Search existing GitHub issues
3. Review logs for error messages
4. Create a new issue with detailed information

## Contributing to Documentation

We welcome documentation improvements! To contribute:

1. Fork the repository
2. Edit documentation files in the `doc/` directory
3. Submit a pull request with your changes
4. Describe what you changed and why

### Documentation Style Guide

- Use clear, concise language
- Include code examples where helpful
- Add screenshots for UI-related content
- Keep formatting consistent
- Test all commands and code samples

## Version Information

This documentation is current for:
- **ACAL Version:** 1.x
- **Last Updated:** January 2026
- **.NET Version:** 10.0+

For version-specific changes, see the [main README](../README.md) and release notes.

## License

ACAL is licensed under the MIT License. See [LICENSE](../LICENSE) for details.

Documentation is licensed under [CC BY 4.0](https://creativecommons.org/licenses/by/4.0/).

## Additional Resources

### External Links
- [ACAL GitHub Repository](https://github.com/ArizonaGreenTea05/ACAL)
- [Docker Hub Image](https://hub.docker.com/r/arizonagreentea0905/acal)
- [.NET Documentation](https://docs.microsoft.com/dotnet/)
- [Blazor Documentation](https://docs.microsoft.com/aspnet/core/blazor/)

### Related Projects
- [Ical.Net](https://github.com/rianjs/ical.net) - ICS parsing library
- [SpotifyAPI-NET](https://github.com/JohnnyCrazy/SpotifyAPI-NET) - Spotify API wrapper

## Acknowledgments

- Built with ❤️ by [ArizonaGreenTea05](https://github.com/ArizonaGreenTea05)
- Powered by .NET and Blazor
- Community contributions welcome

---

**Need help?** Start with the appropriate guide above, or open an issue on GitHub.

**Want to contribute?** Read the [Development Guide](developer/development-guide.md) and join us!
