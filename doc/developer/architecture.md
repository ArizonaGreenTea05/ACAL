# ACAL Architecture Overview

This document provides a high-level overview of the ACAL (ACAL Calendar And Layout) architecture for developers.

## Table of Contents
- [Introduction](#introduction)
- [Technology Stack](#technology-stack)
- [Project Structure](#project-structure)
- [Architecture Principles](#architecture-principles)
- [Component Overview](#component-overview)
- [Data Flow](#data-flow)
- [Key Design Patterns](#key-design-patterns)

## Introduction

ACAL is a Blazor Server application built with .NET 10 that provides an interactive calendar and media display interface. The application follows a clean, layered architecture with clear separation of concerns.

### Key Characteristics

- **Framework:** ASP.NET Core Blazor Server
- **Language:** C# 13 (.NET 10)
- **Architecture:** Modular, layered architecture
- **UI Pattern:** Component-based UI with Blazor components
- **Deployment:** Docker and traditional hosting

## Technology Stack

### Core Technologies

| Technology | Version | Purpose |
|------------|---------|---------|
| .NET | 10.0+ | Runtime and framework |
| ASP.NET Core | 10.0+ | Web framework |
| Blazor Server | 10.0+ | Interactive UI framework |
| C# | 13 | Programming language |

### Key Libraries

| Library | Purpose |
|---------|---------|
| Ical.Net | ICS calendar parsing |
| Serilog | Structured logging |
| SpotifyAPI.Web | Spotify API integration |

### Development Tools

- **Visual Studio 2022** / **Visual Studio Code** - IDEs
- **Docker Desktop** - Containerization
- **MSTest** - Unit testing framework
- **Git** - Version control

## Project Structure

ACAL is organized into multiple projects, each with a specific responsibility:

```
ACAL/
├── CalendarView.Core/              # Core business logic and models
├── CalendarView.Core.Tests/        # Core logic unit tests
├── CalendarView.Services/          # Service layer (calendar, music, pictures)
├── CalendarView.Shared/            # Shared Blazor components and pages
├── CalendarView.Web/               # Main Blazor Server application
├── Common/                         # Cross-cutting utilities
├── Common.UI/                      # Reusable UI utilities
├── Common.UI.Tests/                # UI utility tests
├── Spotify/                        # Spotify authentication and models
└── HelperProjects/                 # Auxiliary tools (e.g., Spotify token helper)
```

### Project Dependency Graph

```
CalendarView.Web
├── CalendarView.Shared
│   ├── CalendarView.Services
│   │   ├── CalendarView.Core
│   │   ├── Spotify
│   │   └── Common
│   ├── Common.UI
│   └── CalendarView.Core
└── Common

CalendarView.Core.Tests → CalendarView.Core
Common.UI.Tests → Common.UI
```

## Architecture Principles

### 1. Separation of Concerns

ACAL is structured in clear layers:

- **Presentation Layer** (`CalendarView.Shared`, `CalendarView.Web`)
  - Blazor components and pages
  - User interface logic
  - Routing and navigation

- **Service Layer** (`CalendarView.Services`)
  - Business logic
  - External API integration
  - Data transformation

- **Domain Layer** (`CalendarView.Core`)
  - Domain models
  - View models
  - Business entities

- **Infrastructure** (`Common`, `Common.UI`, `Spotify`)
  - Cross-cutting concerns
  - Shared utilities
  - Third-party integrations

### 2. Dependency Injection

ACAL uses .NET's built-in dependency injection for:
- Service lifetime management
- Loose coupling between components
- Testability

### 3. Configuration-Driven

Application behavior is controlled through `appsettings.json`:
- Calendar sources
- UI customization
- Authentication settings
- Logging configuration

### 4. Modularity

Features are organized as modules:
- Calendar management
- Picture display
- Spotify integration
- Authentication

Each module can be enabled/disabled or configured independently.

## Component Overview

### CalendarView.Core

**Purpose:** Domain models and core business entities

**Key Components:**
- `CalendarEvent` - Base calendar event model
- `DefaultCalendarEvent` - Standard timed events
- `AllDayCalendarEvent` - All-day events
- `Calendar` - Calendar metadata and customization
- `Notification` - User notification model
- `Enums` - Shared enumerations

**Responsibilities:**
- Define domain entities
- Provide view models for UI
- No external dependencies (pure domain logic)

### CalendarView.Services

**Purpose:** Business logic and external service integration

**Key Components:**
- `CalendarService` - Loads and processes ICS calendars
- `PictureService` - Manages photo display
- `RefreshService` - Handles periodic data refresh
- `SpotifyService` - Spotify API integration
- `MusicService` - Abstract music service interface

**Responsibilities:**
- Fetch data from external sources
- Transform data for UI consumption
- Implement business rules
- Manage service lifecycle

### CalendarView.Shared

**Purpose:** Reusable Blazor components and pages

**Key Components:**

**Pages:**
- `Home.razor` - Landing page
- `Agenda.razor` - Agenda view
- `Calendar.razor` - Calendar view
- `NotFound.razor` - 404 page

**Components:**
- `EventCard.razor` - Event display card
- `DetailedEventCard.razor` - Expanded event view
- `TimeAndDate.razor` - Date/time display
- `ColorLegend.razor` - Calendar color legend
- `SideImage.razor` - Photo display
- `MusicPlayer.razor` - Spotify controls
- `Background.razor` - Background image handler
- `NotificationArea.razor` - Notification display

**Layout:**
- `MainLayout.razor` - Main application layout
- `NavMenu.razor` - Navigation menu

**Responsibilities:**
- Render UI components
- Handle user interaction
- Component state management
- Responsive design

### CalendarView.Web

**Purpose:** Main application host and configuration

**Key Components:**
- `Program.cs` - Application entry point
- `App.razor` - Root Blazor component
- `BasicAuthenticationMiddleware` - HTTP authentication
- `FormFactor` - Device detection service
- `appsettings.json` - Application configuration

**Responsibilities:**
- Configure application services
- Set up middleware pipeline
- Host Blazor application
- Manage application lifecycle

### Common / Common.UI

**Purpose:** Shared utilities and extensions

**Key Components:**
- `TimeSpan` - Time utility extensions
- `ColorExtensions` - Color manipulation
- `DateTimeExtensions` - Date/time utilities

**Responsibilities:**
- Provide reusable utility functions
- Extend built-in types
- Cross-cutting concerns

### Spotify

**Purpose:** Spotify API authentication and models

**Key Components:**
- `Authentication` - OAuth flow
- `SpotifyLoginData` - Login credentials model
- `SpotifyToken` - Token management

**Responsibilities:**
- Handle Spotify authentication
- Manage access tokens
- Define Spotify-specific models

## Data Flow

### Calendar Data Flow

```
External ICS Source
    ↓
CalendarService.LoadEventsFromIcsAsync()
    ↓
Parse ICS using Ical.Net
    ↓
Transform to CalendarEvent models
    ↓
Store in application state
    ↓
Blazor components retrieve and display
```

### Picture Display Flow

```
File System (Image Directory)
    ↓
PictureService.GetRandomPicture()
    ↓
Read image file
    ↓
Convert to base64
    ↓
SideImage component displays
    ↓
RefreshService triggers periodic change
```

### Spotify Integration Flow

```
User Configuration (appsettings.json)
    ↓
SpotifyService initialization
    ↓
Authenticate with Spotify API
    ↓
Poll current playback state
    ↓
MusicPlayer component displays controls
    ↓
User interaction → API calls
    ↓
Update playback state
```

### Configuration Flow

```
appsettings.json
    ↓
Initialization.LoadAppsettings()
    ↓
Parse configuration sections
    ↓
Register services with DI container
    ↓
Services injected into components
    ↓
Components use configuration
```

## Key Design Patterns

### 1. Service Pattern

Services encapsulate business logic and external dependencies:

```csharp
public class CalendarService(HttpClient httpClient, ILogger<CalendarService> logger)
{
    public async Task<List<CalendarEvent>?> LoadEventsFromIcsAsync(string icsUrl, int maxTries = 1)
    {
        // Implementation
    }
}
```

**Benefits:**
- Testable (can mock dependencies)
- Reusable across components
- Clear separation of concerns

### 2. Component Model

Blazor components are self-contained UI units:

```razor
@inject CalendarService CalendarService

<div class="event-card">
    @foreach (var evt in Events)
    {
        <EventCard Event="@evt" />
    }
</div>

@code {
    [Parameter] public List<CalendarEvent> Events { get; set; }
}
```

**Benefits:**
- Encapsulated UI logic
- Reusable components
- Data binding

### 3. Dependency Injection

Services are registered and injected:

```csharp
builder.Services.AddSingleton<CalendarService>();
builder.Services.AddScoped<RefreshService>();
```

**Benefits:**
- Loose coupling
- Testability
- Lifecycle management

### 4. Configuration Pattern

Application behavior driven by configuration:

```csharp
public static void LoadAppsettings(
    out Calendars calendars,
    out Design design,
    out LoggingConfig loggingConfig,
    // ...
)
```

**Benefits:**
- No recompilation for changes
- Environment-specific settings
- Easy customization

### 5. Middleware Pattern

Cross-cutting concerns handled by middleware:

```csharp
app.UseMiddleware<BasicAuthenticationMiddleware>();
```

**Benefits:**
- Centralized authentication
- Request/response pipeline
- Separation of concerns

## Architecture Decisions

### Why Blazor Server?

**Chosen over Blazor WebAssembly because:**
- Direct access to backend services
- No need for API layer
- Better performance for server-side operations
- Simpler deployment model

### Why Modular Structure?

**Benefits:**
- Clear boundaries between concerns
- Easier testing (unit test individual projects)
- Better code organization
- Reusability (Common.UI can be used elsewhere)

### Why Dependency Injection?

**Benefits:**
- Testability (easy to mock dependencies)
- Flexibility (swap implementations)
- .NET standard pattern
- Built-in lifetime management

## Security Considerations

### Authentication

- HTTP Basic Authentication middleware
- Credentials validated on each request
- Browser native login dialog

### Data Security

- Calendar data loaded over HTTPS (recommended)
- Spotify tokens stored in configuration (should be secured)
- No sensitive data persisted to disk (except logs)

### Best Practices

- Use HTTPS in production
- Secure configuration files
- Regular dependency updates
- Input validation on configuration

## Performance Considerations

### Caching

- Calendar events cached until refresh interval
- Images cached in browser
- Blazor component state management

### Optimization Strategies

- Lazy loading of images
- Periodic refresh (not continuous polling)
- Efficient LINQ queries
- Minimal re-rendering

## Scalability

### Current Limitations

- Single-server deployment
- In-memory state (no persistence)
- Limited concurrent users (Blazor Server model)

### Suitable For

- Personal use (1-10 users)
- Kiosk displays
- Home/office displays

### Not Suitable For

- High-traffic public websites
- Multi-tenant SaaS applications
- Real-time collaboration features

## Future Considerations

Potential architectural improvements:

- **State Management:** Add persistent state (database)
- **Caching Layer:** Implement Redis or similar
- **API Layer:** Separate backend API for flexibility
- **WebAssembly Option:** Support Blazor WASM for offline capability
- **Real-time Updates:** Use SignalR for push notifications
- **Plugin System:** Allow custom calendar/music providers

## Conclusion

ACAL's architecture emphasizes:
- **Simplicity:** Easy to understand and modify
- **Modularity:** Clear separation of concerns
- **Testability:** Unit tests for core logic
- **Flexibility:** Configuration-driven behavior
- **Maintainability:** Clean code structure

This architecture supports the application's goal of being a customizable, personal calendar and media display solution while remaining approachable for contributors.
