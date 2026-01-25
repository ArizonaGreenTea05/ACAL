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

                    // Validate and normalize the path to a fully qualified local file path
                    if (!Path.IsPathRooted(filePath) || uri.IsUnc || filePath.StartsWith(@"\\", StringComparison.Ordinal))
                    {
                        logger.LogError("Invalid or unsupported file path in URI: {path}", filePath);
                        throw new ArgumentException($"Invalid or unsupported file path in URI: {filePath}", nameof(icsUrl));
                    }
                    var normalizedPath = Path.GetFullPath(filePath);
                    
                    // Security check: Ensure it's a regular file, not a directory
                    // This will throw FileNotFoundException if the file doesn't exist
                    var attributes = File.GetAttributes(normalizedPath);
                    if ((attributes & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        logger.LogError("Path is a directory, not a file: {path}", normalizedPath);
                        return null;
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