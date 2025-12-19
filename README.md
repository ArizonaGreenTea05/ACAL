# 🚀 ACAL: Your Personalized Digital Hub for Calendar, Photos, and Music

<!-- TODO: Add project logo -->

<div align="center">

[![GitHub release](https://img.shields.io/github/v/release/ArizonaGreenTea05/ACAL)](https://github.com/ArizonaGreenTea05/ACAL/releases/latest)

[![GitHub stars](https://img.shields.io/github/stars/ArizonaGreenTea05/ACAL?style=for-the-badge&logo=github)](https://github.com/ArizonaGreenTea05/ACAL/stargazers)
[![GitHub forks](https://img.shields.io/github/forks/ArizonaGreenTea05/ACAL?style=for-the-badge&logo=github)](https://github.com/ArizonaGreenTea05/ACAL/network)
[![YouTrack issues](https://img.shields.io/badge/dynamic/json?style=for-the-badge&url=https://sugoi.youtrack.cloud/api/issues?query=project:ACAL&query=$.length&label=YouTrack%20issues&color=violet)](https://sugoi.youtrack.cloud/projects/ACAL/agiles/195-1/current)
[![GitHub issues](https://img.shields.io/github/issues/ArizonaGreenTea05/ACAL?style=for-the-badge&logo=github)](https://github.com/ArizonaGreenTea05/ACAL/issues)
[![GitHub license](https://img.shields.io/github/license/ArizonaGreenTea05/ACAL?style=for-the-badge)](LICENSE)

**ACAL (_ACAL Calendar And Layout_) is a versatile and highly customizable web application designed to help users stay organized and enjoy their digital content. Display photos, manage your calendar, and integrate music all in one elegant interface.**

<!-- T[Live Demo](https://demo-link.com) ODO: Add live demo link -->
<!-- [Documentation](https://docs-link.com) TODO: Add documentation link -->

</div>

## 📖 Overview

ACAL provides a modern and adaptable solution for visualizing important events, showcasing personal photo collections, and integrating music playback. Built with Blazor Server, it offers a rich interactive user experience while leveraging the robust capabilities of the .NET ecosystem. Whether you're looking to streamline your daily schedule or create a dynamic digital display, ACAL makes it easy to personalize your space and keep everything important in sight.

## ✨ Features

-   🎯 **Interactive Calendar View**: Visualize and manage events with a customizable calendar interface.
-   📸 **Personalized Photo Display**: Showcase cherished memories and dynamic photo albums.
-   🎶 **Integrated Music Player**: Seamlessly control and enjoy your favorite tunes with Spotify integration.
-   🎨 **Customizable Layouts**: Tailor the application's appearance and content arrangement to your preferences.
-   📈 **Efficient Organization**: Stay on top of your schedule and important activities, ensuring you won't miss a thing.
-   ⚙️ **Modular Architecture**: Built with a clean, project-separated structure for maintainability and extensibility.

## 🖥️ Screenshots

### Agenda horizontal (with image)
<img src="doc/images/AgendaWithImageAndBackground/horizontal.png" alt="Screenshot 1" width="600"/>

### Agenda vertical (with/without image)
<img src="doc/images/AgendaWithBackground/vertical.png" alt="Screenshot 1" width="400"/>
<img src="doc/images/AgendaWithImageAndBackground/vertical.png" alt="Screenshot 1" width="400"/>

### Calendar horizontal (with/without image)
<img src="doc/images/CalendarWithBackground/horizontal.png" alt="Screenshot 1" width="600"/>
<img src="doc/images/CalendarWithImageAndBackground/horizontal.png" alt="Screenshot 1" width="600"/>

## 🛠️ Tech Stack

**Frontend:**
![Blazor](https://img.shields.io/badge/Blazor-512BD4?style=for-the-badge&logo=blazor&logoColor=white)
![HTML5](https://img.shields.io/badge/HTML5-E34F26?style=for-the-badge&logo=html5&logoColor=white)
![CSS3](https://img.shields.io/badge/CSS3-1572B6?style=for-the-badge&logo=css3&logoColor=white)

**Backend:**
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-512BD4?style=for-the-badge&logo=dot-net&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)

**DevOps:**
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)
<!--
**Testing:**
![xUnit](https://img.shields.io/badge/xUnit-8D3E8D?style=for-the-badge&logo=xunit&logoColor=white)-->

## 🚀 Quick Start

Follow these steps to get ACAL up and running on your local machine.

### Prerequisites
Before you begin, ensure you have the following installed:
-   **[.NET SDK](https://dotnet.microsoft.com/download)**: Version 10.0 or later (for Blazor Server development).
-   **[Git](https://git-scm.com/downloads)**: For cloning the repository.
-   **[Docker Desktop](https://www.docker.com/products/docker-desktop/)** (Optional): If you plan to run the application in a container.

### Installation

1.  **Clone the repository**
    ```bash
    git clone https://github.com/ArizonaGreenTea05/ACAL.git
    cd ACAL
    ```

2.  **Restore dependencies and build the solution**
    Navigate to the root of the cloned repository where `CalendarView.slnx` is located.
    ```bash
    dotnet restore
    dotnet build
    ```

3.  **Environment setup (if applicable)**
    ACAL uses `appsettings.json` for configuration.
    ```json
    # Minimal appsettings.json for CalendarView project
    {
      "Logging": {
        "LogLevel": {
          "Default": "Information",
          "Microsoft.AspNetCore": "Warning"
        }
      },
      "AllowedHosts": "*",
      "Design": {
        "PageLayout": "AgendaWithImageAndBackground",
        "LongDateFormat": "dddd, dd. MMMM yyyy",
        "ShortDateFormat": "dd.MM.",
        "ShortTimeFormat": "HH:mm",
        "LongMonthFormat": "MMMM",
        "LongDayFormat": "dddd"
      },
      "LoggingConfig": {
        "LoggingTemplate": "| {Timestamp:HH:mm:ss:fff} | {Level:u3} | {SourceContext} | {CallerMemberName} | {Message:lj} | {CallerFilePath}:{CallerLineNumber} | {Exception} |",
        "LoggingPath": "logs/log.debug",
        "FilteredLoggingPath": "logs/log.information"
      },
      "Calendars": {
        "RefreshAfterMinutes": 60,
        "Definitions": {
          // Colons have to be replaced with pipes, otherwise the json can not be deserialized correctly. So please use "https|//" instead of "https://"
          "https|//ics.tools/Ferien/nordrhein-westfalen.ics": {
            "Color": "#FFFD00",
            "CustomName": "Holidays"
          }
        }
      }
    }
    ```
    
    ```json
    # Recommended appsettings.json for CalendarView project
    {
      "Logging": {
        "LogLevel": {
          "Default": "Information",
          "Microsoft.AspNetCore": "Warning"
        }
      },
      "AllowedHosts": "*",
      "Design": {
        "Language": "en",
        "PictureDirectory": "../images", // a folder mounted to the docker container as "images"
        "ChangePictureAfterMinutes": 0.2,
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
        "Designs": { // Here you can specify design choices for specific layouts
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
        "ClientId": "CLIENT_ID",
        "ClientSecret": "CLIENT_SECRET",
        "AuthToken": { // You can get this part using the spotify helper attached to each release. You still have to define the CLIENT_ID and CLIENT_SECRET in the helper's SpotifyLoginData.json as well as configure it on https://developer.spotify.com/dashboard
          "AccessToken": "ACCESS_TOKEN",
          "RefreshToken": "REFRESH_TOKEN",
          "TokenType": "Bearer",
          "ExpiresIn": 3600,
          "CreatedAt": "2025-12-12T13:35:45.5304927Z"
        }
      },
      "Calendars": {
        "RefreshAfterMinutes": 1,
        "Definitions": {
          // Colons have to be replaced with pipes, otherwise the json can not be deserialized correctly. So please use "https|//" instead of "https://"
          "https|//www.example.com/default.ics": {
            "Color": "#FF0000",
            "CustomName": "Default"
          },
          "https|//www.example.com/tasks.ics": {
            "Color": "#00FF00",
            "CustomName": "Tasks"
          },
          "https|//www.example.com/free-time.ics": {
            "Color": "#FF8000",
            "CustomName": "Free time"
          },
          "https|//www.example.com/birthdays.ics": {
            "Color": "#0000FF",
            "CustomName": "Birthdays"
          },
          "https|//ics.tools/Ferien/nordrhein-westfalen.ics": {
            "Color": "#FFFD00",
            "CustomName": "Holidays"
          },
          "https|//ics.tools/Feiertage/nordrhein-westfalen.ics": {
            "Color": "#FFFD00",
            "CustomName": "Holidays"
          }
        }
      }
    }
    ```

4.  **Start development server**
    To run the main Blazor application, navigate into the `CalendarView` project directory and run:
    ```bash
    cd CalendarView
    dotnet run
    ```
    Alternatively, you can run the solution from the root:
    ```bash
    dotnet run --project CalendarView
    ```

5.  **Open your browser**
    The application will typically start on `http://localhost:5000` (HTTP) and `https://localhost:5001` (HTTPS) by default. Check your console output for the exact URLs.

## 📁 Project Structure

```
ACAL/
├── .dockerignore              # Specifies files to ignore when building Docker images
├── .github/                   # GitHub configuration
├── .gitignore                 # Standard .NET Git ignore file
├── CalendarView.Core.Tests/   # Unit/integration tests for core logic
├── CalendarView.Core/         # Core business logic, models, and shared entities
├── CalendarView.Services/     # Service layer for business operations and data access
├── CalendarView.slnf          # Visual Studio Solution Filter
├── CalendarView.slnx          # Visual Studio Solution file for the entire project
├── CalendarView/              # The main Blazor Server application project
│   ├── CalendarView.Shared/   # Shared Blazor components and layouts and pages
│   │   ├── wwwroot/           # Static assets (CSS, JS, images)
│   │   ├── Components/        # Custom components
│   │   ├── Layout/            # Global layout and styles
│   │   ├── Models/            # Classes needed for the UI
│   │   ├── Pages/             # Page components
│   │   ├── Resources/         # Resources needed for the UI
│   │   ├── Utils/             # Utils needed for the UI
│   │   └── _Imports.razor     # Global imports for all components
│   └── CalendarView.Web/      # The main Blazor Server application project
│       ├── appsettings.json   # Application configuration
│       └── Program.cs         # Application entry point
├── Common.UI.Tests/           # Unit/integration tests for common UI components
├── Common.UI/                 # Reusable UI components for Blazor
├── Common/                    # Shared utilities, helper classes, cross-cutting concerns
├── Directory.Build.props      # Custom MSBuild properties for the solution
├── HelperProjects/            # Auxiliary projects for specific functionalities
├── LICENSE                    # Project license file (GPL-3.0)
├── README.md                  # This README file
└── Spotify/                   # Project specific to Spotify API integration
```

## ⚙️ Configuration

### Application Settings
Configuration for the application is managed via `appsettings.json`.

-   `appsettings.json`: Contains default production-ready configurations.

## 🔧 Development

### Available Scripts
The primary commands for development are executed using the .NET CLI:

| Command                         | Description                               |
|---------------------------------|-------------------------------------------|
| `dotnet restore`                | Restores NuGet packages for all projects. |
| `dotnet build`                  | Builds the entire solution.               |
| `dotnet run --project [Project]`| Runs a specific project (e.g., `CalendarView`). |
| `dotnet test`                   | Runs all tests in the solution.           |
| `dotnet publish`                | Publishes the application for deployment. |

### Development Workflow
1.  **Code Changes**: Make changes within the relevant project directories (`CalendarView.Core`, `CalendarView.Services`, `CalendarView`, `Common.UI`, `Spotify`, `Common`).
2.  **Build**: Use `dotnet build` to compile your changes.
3.  **Run**: Use `dotnet run --project CalendarView` to test the application locally.
4.  **Test**: Execute `dotnet test` to ensure all tests pass.

## 🧪 Testing

ACAL includes unit and integration tests to ensure reliability and correctness. Test projects are located in `CalendarView.Core.Tests` and `Common.UI.Tests`.

To run all tests in the solution:
```bash
dotnet test
```

## 🚀 Deployment

### Production Build
To create a production-ready build of the `CalendarView` application:
```bash
cd CalendarView
dotnet publish -c Release -o ../publish
```
This command will compile your application in Release mode and place the publishable output in the `publish` directory at the root level.

### Deployment Options

-   **Docker**: A Docker image is automatically pushed to [`arizonagreentea0905/acal`](https://hub.docker.com/r/arizonagreentea0905/acal) when a new release is created. For deployment the container only requires a folder mapped as "config" that contains the `appsettings.json` file.
-   **Traditional Hosting**: The `dotnet publish` output can be deployed to various hosting environments, including IIS on Windows, Apache/Nginx on Linux, or cloud services like Azure App Service, AWS Elastic Beanstalk, or shared hosting that supports .NET Core applications.

## 🤝 Contributing

We welcome contributions to ACAL! If you're interested in improving the project, please follow these guidelines:

1.  **Fork the repository**.
2.  **Create a new branch** for your feature or bug fix.
    ```bash
    git checkout -b feat/your-feature-name
    ```
3.  **Make your changes**, ensuring they adhere to the project's coding standards.
4.  **Write and run tests** to cover your changes.
5.  **Commit your changes** with a clear and descriptive commit message.
6.  **Push your branch** to your forked repository.
7.  **Open a Pull Request** to the `main` branch of this repository.

**Note:** If you are interested in contributing to this project on a regular basis and/or improving it in the long term, you can request access to the YouTrack board so that we can collaborate as a team.

### Development Setup for Contributors
The development setup is identical to the Quick Start instructions provided above. Please ensure you have the .NET SDK and any relevant tools installed.

## 📄 License

This project is licensed under the [MIT License](LICENSE) - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

-   Built with the powerful **.NET** platform and **Blazor** framework.
-   Integrates with the **Spotify API** for music functionalities.
-   Inspired by the need for customizable and integrated digital displays.

## 📞 Support & Contact

-   🐛 Issues:
    - Community: [GitHub Issues](https://github.com/ArizonaGreenTea05/ACAL/issues)
    - Developers: [YouTrack Issues](https://sugoi.youtrack.cloud/projects/ACAL/agiles/195-1/current)
-   📧 Contact: [ArizonaGreenTea05](https://github.com/ArizonaGreenTea05) <!-- TODO: Add a more specific contact email if available -->

---

<div align="center">

**⭐ Star this repo if you find it helpful!**

Made with ❤️ by [ArizonaGreenTea05](https://github.com/ArizonaGreenTea05)

</div>