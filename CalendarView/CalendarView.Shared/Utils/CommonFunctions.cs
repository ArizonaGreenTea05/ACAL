using System.Drawing;
using CalendarView.Core.Models;
using Common.UI.Extensions;
using Microsoft.Extensions.Logging;

namespace CalendarView.Shared.Utils;

public static class CommonFunctions
{
    public static void LoadPictures(ILogger logger, string? directory, List<string> imageBase64s)
    {
        logger.LogInformation("Started loading pictures");
        var tempBase64s = new List<string>();
        if (!Directory.Exists(directory))
        {
            logger.LogWarning("Picture directory does not exist: {path}", directory);
            return;
        }
        tempBase64s.AddRange(GetImageBase64s(Directory.EnumerateFiles(directory)));

        var subDirectories = Directory.EnumerateDirectories(directory).ToList();
        var matches = subDirectories.Where(d => MatchesCurrentDate(Path.GetFileName(d))).ToList();
        if (matches.Count <= 0)
        {
            var defaultImages = subDirectories.FirstOrDefault(d => Path.GetFileName(d) == "default");
            if (defaultImages is not null) matches.Add(defaultImages);
        }

        foreach (var match in matches)
        {
            tempBase64s.AddRange(GetImageBase64s(Directory.EnumerateFiles(match)));
        }

        imageBase64s.Clear();
        var random = new Random();
        imageBase64s.AddRange(tempBase64s.OrderBy(_ => random.Next()));

        logger.LogInformation("Finished loading pictures");
    }

    private static bool MatchesCurrentDate(string folderName)
    {
        var dateStrings = folderName.Split('-', StringSplitOptions.RemoveEmptyEntries);

        var firstDate = GetDate(dateStrings[0]);
        if (firstDate is null) return false;
        var firstDateAsDateTime = new DateTime(firstDate.Value.year ?? DateTime.Today.Year,
            firstDate.Value.month, firstDate.Value.day);
        if (dateStrings.Length <= 1) return DateTime.Today == firstDateAsDateTime;
        var secondDate = GetDate(dateStrings[1]);
        if (secondDate is null) return false;
        var secondDateAsDateTime = new DateTime(secondDate.Value.year ?? DateTime.Today.Year,
            secondDate.Value.month, secondDate.Value.day);

        var options = new List<(DateTime start, DateTime end)>();

        if (firstDateAsDateTime < secondDateAsDateTime)
        {
            options.Add((firstDateAsDateTime, secondDateAsDateTime));
        }
        else
        {
            options.Add((new DateTime(firstDateAsDateTime.Year - 1, firstDateAsDateTime.Month, firstDateAsDateTime.Day), secondDateAsDateTime));
            options.Add((firstDateAsDateTime, new DateTime(secondDateAsDateTime.Year + 1, secondDateAsDateTime.Month, secondDateAsDateTime.Day)));
        }

        return options.Any(o => o.start <= DateTime.Now.Date && DateTime.Now.Date <= o.end);
    }

    private static (int day, int month, int? year)? GetDate(string dateString)
    {
        var split = dateString.Split('.');
        if (split.Length < 2
            || !int.TryParse(split[0], out var day)
            || !int.TryParse(split[1], out var month)) return null;
        if (split.Length < 3
            || !int.TryParse(split[2], out var year)) return (day, month, null);
        return (day, month, year);
    }

    private static IEnumerable<string> GetImageBase64s(IEnumerable<string> files)
    {
        return files.Where(f => MimeMapping.MimeUtility.GetMimeMapping(f).StartsWith("image/"))
            .Select(f => Convert.ToBase64String(File.ReadAllBytes(f)));
    }

    public static bool IsEventAtDay(CalendarEvent ev, DateTime day)
    {
        return ev.TotalStartTime.Date <= day && ev.TotalEndTime.Date >= day;
    }

    public static IEnumerable<(Calendar calendar, Color backColor, Color dimBackColor, Color foreColor)> GetCalendarsOrderedByColor(IEnumerable<Calendar> calendars, double eventCardDimmingRatio)
    {
        return calendars
            .OrderBy(c => c.Color.GetHue())
            .ThenBy(c => c.Color.R * 3 + c.Color.G * 2 + c.Color.B * 1)
            .Select((Calendar calendar, Color dimBackColor) (calendar) => (calendar, calendar.Color.GetDimColor(eventCardDimmingRatio)))
            .Select(calendar => (calendar.calendar, calendar.calendar.Color, calendar.dimBackColor, calendar.dimBackColor.GetForeColor()));
    }
}