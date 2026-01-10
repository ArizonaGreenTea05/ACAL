# Development Guide

This guide helps developers set up their development environment and contribute to ACAL.

## Table of Contents
- [Prerequisites](#prerequisites)
- [Development Environment Setup](#development-environment-setup)
- [Building the Project](#building-the-project)
- [Running Tests](#running-tests)
- [Development Workflow](#development-workflow)
- [Code Style Guidelines](#code-style-guidelines)
- [Debugging](#debugging)
- [Contributing](#contributing)

## Prerequisites

### Required Software

1. **.NET SDK 10.0 or later**
   - Download from [dotnet.microsoft.com](https://dotnet.microsoft.com/download)
   - Verify installation:
     ```bash
     dotnet --version
     ```

2. **Git**
   - Download from [git-scm.com](https://git-scm.com/downloads)
   - Verify installation:
     ```bash
     git --version
     ```

3. **IDE (Choose one)**
   - **Visual Studio 2022** (v17.12+) - Recommended for Windows
     - Workload: ASP.NET and web development
     - Workload: .NET desktop development
   - **Visual Studio Code** - Recommended for macOS/Linux
     - Extension: C# Dev Kit
     - Extension: C# Extensions

4. **Docker Desktop** (Optional, for container testing)
   - Download from [docker.com](https://www.docker.com/products/docker-desktop/)

### Recommended Tools

- **Git GUI Client** (GitKraken, GitHub Desktop, SourceTree)
- **Postman** or **curl** (for API testing)
- **JSON Validator** (for configuration validation)

## Development Environment Setup

### 1. Clone the Repository

```bash
git clone https://github.com/ArizonaGreenTea05/ACAL.git
cd ACAL
```

### 2. Restore Dependencies

```bash
dotnet restore
```

This will restore all NuGet packages for all projects in the solution.

### 3. Build the Solution

```bash
dotnet build
```

Verify that all projects build successfully.

### 4. Configure Development Settings

Create or modify `CalendarView/CalendarView.Web/appsettings.Development.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.AspNetCore": "Information"
    }
  },
  "AuthenticationConfig": {
    "Enabled": false
  },
  "Design": {
    "PageLayout": "AgendaWithImageAndBackground"
  },
  "Calendars": {
    "RefreshAfterMinutes": 1,
    "Definitions": {
      "https|//ics.tools/Ferien/nordrhein-westfalen.ics": {
        "Color": "#FFFD00",
        "CustomName": "Test Calendar"
      }
    }
  }
}
```

**Note:** Development settings override production settings from `appsettings.json`.

### 5. Run the Application

```bash
cd CalendarView/CalendarView.Web
dotnet run
```

Or from the solution root:

```bash
dotnet run --project CalendarView/CalendarView.Web
```

The application will start on:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`

### 6. Verify Setup

Open your browser and navigate to `http://localhost:5000`. You should see the ACAL home page.

## Building the Project

### Build All Projects

```bash
# From solution root
dotnet build
```

### Build Specific Project

```bash
dotnet build CalendarView/CalendarView.Web
```

### Build for Release

```bash
dotnet build -c Release
```

### Clean Build

```bash
dotnet clean
dotnet build
```

### Build with Specific Framework

```bash
dotnet build -f net10.0
```

## Running Tests

ACAL includes unit tests for core functionality.

### Run All Tests

```bash
dotnet test
```

### Run Tests for Specific Project

```bash
dotnet test CalendarView.Core.Tests
dotnet test Common.UI.Tests
```

### Run Tests with Detailed Output

```bash
dotnet test --verbosity detailed
```

### Run Tests with Code Coverage

```bash
dotnet test /p:CollectCoverage=true /p:CoverageReportFormat=html
```

### Run Specific Test

```bash
dotnet test --filter "FullyQualifiedName~CalendarTests"
```

### Watch Mode (Auto-run on change)

```bash
dotnet watch test
```

## Development Workflow

### 1. Create a Feature Branch

```bash
git checkout -b feat/your-feature-name
```

**Branch Naming Conventions:**
- `feat/` - New features
- `fix/` - Bug fixes
- `docs/` - Documentation changes
- `refactor/` - Code refactoring
- `test/` - Test additions/changes

### 2. Make Changes

Edit code in your preferred IDE. Follow the [Code Style Guidelines](#code-style-guidelines).

### 3. Build and Test

```bash
# Build to check for compilation errors
dotnet build

# Run tests to verify functionality
dotnet test

# Run the application to test manually
dotnet run --project CalendarView/CalendarView.Web
```

### 4. Commit Changes

```bash
git add .
git commit -m "feat: add calendar color customization"
```

**Commit Message Format:**
```
<type>: <description>

[optional body]

[optional footer]
```

**Types:**
- `feat` - New feature
- `fix` - Bug fix
- `docs` - Documentation
- `style` - Code style changes (formatting)
- `refactor` - Code refactoring
- `test` - Adding tests
- `chore` - Maintenance tasks

### 5. Push Changes

```bash
git push origin feat/your-feature-name
```

### 6. Create Pull Request

- Go to GitHub repository
- Click "Pull Request"
- Select your branch
- Fill in description
- Submit for review

## Code Style Guidelines

### General Principles

- **Clarity over cleverness** - Write readable code
- **Consistency** - Follow existing patterns
- **Simplicity** - Keep it simple
- **Documentation** - Comment complex logic

### C# Style Guidelines

#### Naming Conventions

```csharp
// Classes and interfaces - PascalCase
public class CalendarService { }
public interface IFormFactor { }

// Methods - PascalCase
public void LoadCalendarEvents() { }

// Properties - PascalCase
public string CalendarName { get; set; }

// Private fields - _camelCase
private readonly ILogger _logger;

// Local variables - camelCase
var eventList = new List<CalendarEvent>();

// Constants - PascalCase
public const int MaxRetries = 3;
```

#### Code Organization

```csharp
public class ExampleClass
{
    // 1. Constants
    public const string DefaultColor = "#FF0000";
    
    // 2. Static fields
    private static readonly HttpClient _httpClient = new();
    
    // 3. Instance fields
    private readonly ILogger<ExampleClass> _logger;
    
    // 4. Constructors
    public ExampleClass(ILogger<ExampleClass> logger)
    {
        _logger = logger;
    }
    
    // 5. Properties
    public string Name { get; set; }
    
    // 6. Public methods
    public void DoSomething() { }
    
    // 7. Private methods
    private void HelperMethod() { }
}
```

#### Modern C# Features

Use modern C# features when appropriate:

```csharp
// Primary constructors (C# 12)
public class CalendarService(HttpClient httpClient, ILogger<CalendarService> logger)
{
    // Use parameters directly
}

// Record types for immutable data
public record CalendarEvent(string Title, DateTime Start, DateTime End);

// Pattern matching
var message = result switch
{
    Success => "Operation succeeded",
    Error => "Operation failed",
    _ => "Unknown status"
};

// Null-coalescing
var name = calendarName ?? "Default Calendar";

// Collection expressions (C# 12)
List<string> names = ["Alice", "Bob", "Charlie"];
```

#### Async/Await

```csharp
// Use async/await for I/O operations
public async Task<List<CalendarEvent>> LoadEventsAsync(string url)
{
    var response = await httpClient.GetStringAsync(url);
    return ParseEvents(response);
}

// Name async methods with Async suffix
public async Task<int> CalculateSumAsync(int a, int b)
{
    await Task.Delay(100); // Simulated async work
    return a + b;
}
```

#### LINQ

```csharp
// Use LINQ for collection operations
var upcomingEvents = events
    .Where(e => e.Start > DateTime.Now)
    .OrderBy(e => e.Start)
    .Take(10)
    .ToList();
```

### Blazor Component Guidelines

#### Component Structure

```razor
@* 1. Directives *@
@page "/calendar"
@inject CalendarService CalendarService
@inject ILogger<Calendar> Logger

@* 2. Markup *@
<div class="calendar-container">
    <h1>@Title</h1>
    
    @if (loading)
    {
        <LoadingSpinner />
    }
    else
    {
        <EventList Events="@events" />
    }
</div>

@* 3. Code block *@
@code {
    // Parameters
    [Parameter]
    public string Title { get; set; } = "Calendar";
    
    // Fields
    private bool loading = true;
    private List<CalendarEvent> events = [];
    
    // Lifecycle methods
    protected override async Task OnInitializedAsync()
    {
        await LoadEventsAsync();
    }
    
    // Methods
    private async Task LoadEventsAsync()
    {
        loading = true;
        events = await CalendarService.LoadEventsAsync();
        loading = false;
    }
}
```

#### Component Parameters

```razor
@code {
    // Required parameter
    [Parameter, EditorRequired]
    public string CalendarId { get; set; } = null!;
    
    // Optional parameter with default
    [Parameter]
    public string Color { get; set; } = "#FF0000";
    
    // Event callback
    [Parameter]
    public EventCallback<CalendarEvent> OnEventSelected { get; set; }
}
```

### CSS Guidelines

```css
/* Use BEM naming convention */
.event-card {
    /* Block */
}

.event-card__title {
    /* Element */
}

.event-card--highlighted {
    /* Modifier */
}

/* Use CSS custom properties for theming */
:root {
    --primary-color: #007bff;
    --secondary-color: #6c757d;
}

.button {
    background-color: var(--primary-color);
}
```

## Debugging

### Visual Studio Debugging

1. Set breakpoints by clicking in the left margin
2. Press F5 to start debugging
3. Use debug toolbar to step through code

**Useful Windows:**
- **Locals** - View local variables
- **Watch** - Watch specific expressions
- **Call Stack** - View call hierarchy
- **Output** - View debug output

### Visual Studio Code Debugging

1. Install C# Dev Kit extension
2. Open Run and Debug view (Ctrl+Shift+D)
3. Select ".NET Core Launch (web)" configuration
4. Press F5 to start debugging

### Browser Developer Tools

For debugging Blazor UI:

1. Open browser developer tools (F12)
2. Check Console tab for JavaScript errors
3. Use Network tab to monitor requests
4. Inspect elements in Elements/Inspector tab

### Logging

Add detailed logging in your code:

```csharp
public class CalendarService(ILogger<CalendarService> logger)
{
    public async Task LoadEventsAsync()
    {
        logger.LogInformation("Loading calendar events");
        
        try
        {
            // Your code
            logger.LogDebug("Loaded {Count} events", events.Count);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to load events");
        }
    }
}
```

**Log Levels:**
- `Trace` - Very detailed logs
- `Debug` - Debug information
- `Information` - General information
- `Warning` - Warning messages
- `Error` - Error messages
- `Critical` - Critical failures

### Hot Reload

.NET supports hot reload for rapid development:

```bash
dotnet watch run --project CalendarView/CalendarView.Web
```

Changes to C# and Razor files will automatically reload without restarting.

## Contributing

### Before Submitting a PR

- [ ] Code builds successfully
- [ ] All tests pass
- [ ] New features include tests
- [ ] Code follows style guidelines
- [ ] Documentation is updated
- [ ] Commit messages are clear
- [ ] No sensitive data in code

### Pull Request Template

```markdown
## Description
Brief description of changes

## Type of Change
- [ ] Bug fix
- [ ] New feature
- [ ] Documentation update
- [ ] Code refactoring

## Testing
Describe testing performed

## Screenshots (if applicable)
Add screenshots for UI changes

## Checklist
- [ ] Code builds successfully
- [ ] Tests pass
- [ ] Documentation updated
```

### Code Review Process

1. Submit pull request
2. Automated checks run (build, tests)
3. Code review by maintainers
4. Address feedback
5. Approval and merge

### Getting Help

- **GitHub Issues** - Report bugs or request features
- **Discussions** - Ask questions
- **YouTrack** - Track development tasks
- **Code Comments** - Ask specific questions in PR comments

## Best Practices

### Performance

- Use async/await for I/O operations
- Avoid blocking calls
- Cache frequently accessed data
- Use efficient LINQ queries
- Minimize component re-renders

### Security

- Validate all inputs
- Sanitize user data
- Use parameterized queries (if adding DB)
- Keep dependencies updated
- Don't commit secrets

### Testing

- Write tests for business logic
- Test edge cases
- Use mocking for external dependencies
- Aim for good code coverage
- Write readable test names

### Documentation

- Comment complex algorithms
- Document public APIs
- Update README for major changes
- Add XML documentation comments
- Keep documentation up-to-date

## Common Development Tasks

### Add a New Page

1. Create `.razor` file in `CalendarView.Shared/Pages/`
2. Add `@page` directive with route
3. Implement component logic
4. Add navigation link if needed

### Add a New Service

1. Create class in `CalendarView.Services/`
2. Implement service logic
3. Register service in `Program.cs`:
   ```csharp
   builder.Services.AddSingleton<YourService>();
   ```
4. Inject where needed:
   ```razor
   @inject YourService YourService
   ```

### Add a New Component

1. Create `.razor` file in `CalendarView.Shared/Components/`
2. Define component markup and logic
3. Use in pages:
   ```razor
   <YourComponent />
   ```

### Add a Configuration Option

1. Add property to configuration model
2. Update `appsettings.json` schema
3. Load in `Initialization.LoadAppsettings()`
4. Use in services/components
5. Document in user documentation

## Resources

### Official Documentation

- [.NET Documentation](https://docs.microsoft.com/dotnet/)
- [Blazor Documentation](https://docs.microsoft.com/aspnet/core/blazor/)
- [C# Language Reference](https://docs.microsoft.com/dotnet/csharp/)

### Community Resources

- [Stack Overflow](https://stackoverflow.com/questions/tagged/blazor)
- [Blazor University](https://blazor-university.com/)
- [.NET Blog](https://devblogs.microsoft.com/dotnet/)

### ACAL Resources

- [GitHub Repository](https://github.com/ArizonaGreenTea05/ACAL)
- [Issue Tracker](https://github.com/ArizonaGreenTea05/ACAL/issues)
- [YouTrack Board](https://sugoi.youtrack.cloud/projects/ACAL/agiles/195-1/current)
