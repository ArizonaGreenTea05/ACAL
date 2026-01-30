using Ical.Net.CalendarComponents;
using Ical.Net;
using Microsoft.Extensions.Logging;

namespace CalendarView.Services;

public class CalendarService(HttpClient httpClient, ILogger<CalendarService> logger)
{
    /// <summary>
    /// Loads calendar events from an ICS source.
    /// </summary>
    /// <param name="icsSource">The URL or file URI to load the ICS calendar from. 
    /// Supports HTTP/HTTPS URLs (e.g., "https://example.com/calendar.ics") 
    /// and file URIs (e.g., "file:///path/to/calendar.ics").</param>
    /// <param name="cancellationToken">A token that can be used to cancel the calendar loading operation.</param>
    /// <param name="maxTries">The maximum number of attempts to load the calendar (default: 1).</param>
    /// <returns>A list of calendar events, or null if loading fails.</returns>
    public async Task<List<CalendarEvent>?> LoadEventsFromIcsAsync(string icsSource, CancellationToken cancellationToken, int maxTries = 1)
    {
        if (maxTries < 1)
        {
            logger.LogWarning("maxTries ({maxTries}) is less than 1. Defaulting to 1 attempt.", maxTries);
            maxTries = 1;
        }

        Uri uri;

        try
        {
            uri = new Uri(icsSource);
        }
        catch (ArgumentNullException ex)
        {
            logger.LogError("Invalid URL: {message}", ex.Message);
            return null;
        }
        catch (UriFormatException ex)
        {
            logger.LogError("Invalid URL: {message}", ex.Message);
            return null;
        }

        for (var i = 0; i < maxTries; i++)
        {
            if (cancellationToken.IsCancellationRequested) return null;
            try
            {
                string icsData;
                if (uri.IsFile)
                {
                    var filePath = uri.LocalPath;

                    // Validate the path: must be rooted and not a UNC path
                    // Note: UNC paths (e.g., \\server\share) are intentionally not supported for security reasons
                    if (!Path.IsPathRooted(filePath) || uri.IsUnc)
                    {
                        logger.LogError("Invalid or unsupported file path in URI (non-rooted or UNC path): {path}", filePath);
                        return null;
                    }

                    // Normalize the path to resolve any relative path components
                    // Note: Path.GetFullPath is used only for normalization and logging. This service intentionally
                    // allows access to any absolute, non-UNC, non-directory local file path that the process can read;
                    // it does not restrict access to a specific base directory, so callers must be trusted.
                    var normalizedPath = Path.GetFullPath(filePath);

                    // Security check: Ensure the path points to an existing regular file
                    if (!File.Exists(normalizedPath))
                    {
                        logger.LogError("File does not exist: {path}", normalizedPath);
                        return null;
                    }

                    if (Directory.Exists(normalizedPath))
                    {
                        logger.LogError("Path is a directory, not a file: {path}", normalizedPath);
                        return null;
                    }

                    // Check for symbolic links and reparse points
                    var fileInfo = new FileInfo(normalizedPath);
                    if (fileInfo.Attributes.HasFlag(FileAttributes.ReparsePoint))
                    {
                        logger.LogWarning("Path is a symbolic link or reparse point: {path}. Proceeding with caution.", normalizedPath);
                    }

                    try
                    {
                        // Read the file content. Uses UTF-8 encoding with BOM detection by default,
                        // which is consistent with HttpClient.GetStringAsync behavior for HTTP sources.
                        icsData = await File.ReadAllTextAsync(normalizedPath, cancellationToken);
                        logger.LogInformation("Loaded calendar from local file: {path}", normalizedPath);
                    }
                    catch (FileNotFoundException ex)
                    {
                        logger.LogError(ex, "Local calendar file not found: {path}", normalizedPath);
                        return null;
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        logger.LogError(ex, "Access denied when reading local calendar file: {path}", normalizedPath);
                        return null;
                    }
                    catch (IOException ex)
                    {
                        logger.LogError(ex, "I/O error when reading local calendar file: {path}", normalizedPath);
                        // Allow the outer retry loop to attempt again for potentially transient I/O errors
                        continue;
                    }
                }
                else
                {
                    icsData = await httpClient.GetStringAsync(icsSource, cancellationToken);
                    logger.LogInformation("Loaded calendar from URL: {url}", icsSource);
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