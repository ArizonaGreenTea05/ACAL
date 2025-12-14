using Ical.Net.CalendarComponents;
using Ical.Net;
using Microsoft.Extensions.Logging;

namespace CalendarView.Services;

public class CalendarService(HttpClient httpClient, ILogger<CalendarService> logger)
{
    public async Task<List<CalendarEvent>?> LoadEventsFromIcsAsync(string icsUrl, int maxTries = 1)
    {
        for (var i = 0; i < maxTries; i++)
        {
            try
            {
                var icsData = await httpClient.GetStringAsync(icsUrl);
                var calendar = Calendar.Load(icsData);
                logger.LogInformation("Loaded calendar from {url}", icsUrl);
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