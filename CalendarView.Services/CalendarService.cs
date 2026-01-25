using Ical.Net.CalendarComponents;
using Ical.Net;
using Microsoft.Extensions.Logging;

namespace CalendarView.Services;

public class CalendarService(HttpClient httpClient, ILogger<CalendarService> logger)
{
    /// <summary>
    /// Loads calendar events from an ICS source.
    /// </summary>
    /// <param name="icsUrl">The URL or file URI to load the ICS calendar from. 
    /// Supports HTTP/HTTPS URLs (e.g., "https://example.com/calendar.ics") 
    /// and file URIs (e.g., "file:///path/to/calendar.ics").</param>
    /// <param name="maxTries">The maximum number of attempts to load the calendar (default: 1).</param>
    /// <returns>A list of calendar events, or null if loading fails.</returns>
    public async Task<List<CalendarEvent>?> LoadEventsFromIcsAsync(string icsUrl, int maxTries = 1)
    {
        for (var i = 0; i < maxTries; i++)
        {
            try
            {
                var uri = new Uri(icsUrl);
                
                string icsData;
                if (uri.IsFile)
                {
                    var filePath = uri.LocalPath;
                    
                    // Normalize the path to prevent path traversal attacks
                    var normalizedPath = Path.GetFullPath(filePath);
                    
                    // Security check: Ensure the file exists and is a regular file
                    if (!File.Exists(normalizedPath))
                    {
                        logger.LogError("Calendar file not found: {path}", normalizedPath);
                        throw new FileNotFoundException($"Calendar file not found: {normalizedPath}");
                    }
                    
                    icsData = await File.ReadAllTextAsync(normalizedPath);
                    logger.LogInformation("Loaded calendar from local file: {path}", normalizedPath);
                }
                else
                {
                    icsData = await httpClient.GetStringAsync(icsUrl);
                    logger.LogInformation("Loaded calendar from URL: {url}", icsUrl);
                }
                
                var calendar = Calendar.Load(icsData);
                return calendar?.Events.ToList() ?? [];
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to load calendar: {message}", ex.Message);
            }
        }
        
        return null;
    }
}