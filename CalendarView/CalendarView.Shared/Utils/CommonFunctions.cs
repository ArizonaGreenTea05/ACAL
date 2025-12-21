using CalendarView.Core.Models;
using CalendarView.Shared.Resources;
using Common.UI.Extensions;
using System.Drawing;
using System.Globalization;
using Calendar = CalendarView.Core.Models.Calendar;

namespace CalendarView.Shared.Utils;

public static class CommonFunctions
{
    public static void InitCalendarParams(IEnumerable<CalendarEvent> unorderedEvents, out List<CalendarEvent> events, out DateTime startDay, out DateTime endDay, out DateTime today, out DateTime yesterday, out DateTime tomorrow)
    {
        events = [.. unorderedEvents.OrderBy(e => e.TotalStartTime)];
        startDay = DateTime.Now.Date;
        endDay = events.Count <= 0 ? startDay : events.Max(ev => ev.TotalStartTime.Date);

        today = DateTime.Now.Date;
        yesterday = today.Subtract(TimeSpan.FromDays(1));
        tomorrow = today.AddDays(1);
    }

    public static void InitCalendarParamsForDay(IEnumerable<CalendarEvent> events, DateTime day, DateTime today, DateTime tomorrow, DateTime yesterday, CultureInfo currentCulture, string? overwriteLongDayFormat, out List<CalendarEvent> todaysEvents, out List<AllDayCalendarEvent> allDayEvents, out List<DefaultCalendarEvent> normalDayEvents, out string currentDayName)
    {
        todaysEvents = [.. events.Where(ev => IsEventAtDay(ev, day))];
        allDayEvents = [.. todaysEvents.OfType<AllDayCalendarEvent>().OrderBy(ev => ev.Start)];
        normalDayEvents = [.. todaysEvents.OfType<DefaultCalendarEvent>().OrderBy(ev => ev.Start)];
        string? currentDayNameTemp = null;
        if (day == today) currentDayNameTemp = Translations.ResourceManager.GetString(nameof(Translations.Today), currentCulture);
        else if (day == tomorrow) currentDayNameTemp = Translations.ResourceManager.GetString(nameof(Translations.Tomorrow), currentCulture);
        else if (day == yesterday) currentDayNameTemp = Translations.ResourceManager.GetString(nameof(Translations.Yesterday), currentCulture);
        currentDayName = currentDayNameTemp ?? day.ToLongDayString(overwriteLongDayFormat, currentCulture);
    }

    public static bool IsEventAtDay(CalendarEvent ev, DateTime day)
    {
        return ev.TotalStartTime.Date <= day && ev.TotalEndTime.Date >= day;
    }

    public static bool IsVideo(string url) =>
        url.StartsWith("data:video/", StringComparison.OrdinalIgnoreCase)
        || url.StartsWith("data:application/mp4", StringComparison.OrdinalIgnoreCase)
        || url.EndsWith(".mp4", StringComparison.OrdinalIgnoreCase)
        || url.EndsWith(".webm", StringComparison.OrdinalIgnoreCase);

    public static IEnumerable<(Calendar calendar, Color backColor, Color dimBackColor, Color foreColor)> GetCalendarsOrderedByColor(IEnumerable<Calendar> calendars, double eventCardDimmingRatio)
    {
        return calendars
            .OrderBy(c => c.Color.GetHue())
            .ThenBy(c => c.Color.R * 3 + c.Color.G * 2 + c.Color.B * 1)
            .Select((Calendar calendar, Color dimBackColor) (calendar) => (calendar, calendar.Color.GetDimColor(eventCardDimmingRatio)))
            .Select(calendar => (calendar.calendar, calendar.calendar.Color, calendar.dimBackColor, calendar.dimBackColor.GetForeColor()));
    }

    private static readonly Dictionary<Calendar, (Color backColor, Color dimBackColor, Color foreColor, Color dimForeColor)> ColorsByCalendar = new();
    public static (Color backColor, Color dimBackColor, Color foreColor, Color dimForeColor) GetColorsOfCalendar(CalendarEvent calendarEvent, double eventCardDimmingRatio)
    {
        if (ColorsByCalendar.TryGetValue(calendarEvent.Calendar, out var colors)) return colors;
        colors.backColor = calendarEvent.Calendar.Color;
        colors.dimBackColor = colors.backColor.GetDimColor(eventCardDimmingRatio);
        colors.foreColor = colors.dimBackColor.GetForeColor();
        colors.dimForeColor = colors.foreColor.GetDimColor(0.9);
        ColorsByCalendar.Add(calendarEvent.Calendar, colors);

        return colors;
    }
}