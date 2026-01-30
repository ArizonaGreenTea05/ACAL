using System.Collections.ObjectModel;
using System.Drawing;
using CalendarView.Core.Models;
using CalendarView.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Ical.Net.DataTypes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CalendarEvent = CalendarView.Core.Models.CalendarEvent;
using IcalCalenderEvent = Ical.Net.CalendarComponents.CalendarEvent;

namespace CalendarView.Core.ViewModels;

public partial class CalendarViewModel(CalendarService calendarService, Calendars sourceCalendars, ILogger<CalendarViewModel> logger) : ObservableObject
{
    public event EventHandler? RefreshedCalendars;

    private Timer? _refreshTimer;

    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private ObservableCollection<CalendarEvent> _events = [];
    [ObservableProperty] private ObservableCollection<Calendar> _calendars = [];

    [ObservableProperty] private ObservableCollection<Notification> _notifications = [];

    public void StartRefreshTimer()
    {
        if (_refreshTimer is not null)
        {
            logger.LogInformation("Refresh timer already running");
            return;
        }
        _refreshTimer = new Timer(RefreshTimerCallback, null, TimeSpan.Zero, TimeSpan.FromMinutes(sourceCalendars.RefreshAfterMinutes));
        logger.LogInformation("Refresh timer started");
    }

    private void RefreshTimerCallback(object? state)
    {
        logger.LogInformation("Refresh timer executing");
        Task.Run(async () => await LoadCalendars());
    }

    private async Task LoadCalendars()
    {
        logger.LogInformation("Started calendar loading");
        if (IsLoading)
        {
            logger.LogInformation("Calendars are already loading");
            return;
        }
        IsLoading = true;

        Events.Clear();
        Calendars.Clear();
        logger.LogDebug("Cleared calendars and events");

        foreach (var calendar in sourceCalendars.Definitions)
        {
            var currentCalendar = new Calendar
            {
                Color = calendar.Value.Color is null ? Color.Gray : ColorTranslator.FromHtml(calendar.Value.Color),
                Name = calendar.Value.CustomName,
                ShowLocation = calendar.Value.ShowLocation
            };

            var fodCal = Calendars.FirstOrDefault(c => c == currentCalendar);
            if (fodCal is null) Calendars.Add(currentCalendar);
            else currentCalendar = fodCal;
            logger.LogDebug("Added calendar: {json}", JsonConvert.SerializeObject(currentCalendar));

            var events = await calendarService.LoadEventsFromIcsAsync(calendar.Key, 4);

            if (events is null)
            {
                var message = $"Failed to load events for calendar: {calendar.Value.CustomName ?? calendar.Key}";
                logger.LogError(message);
                Notifications.Add(new Notification(Enums.NotificationKind.Error, message));
                continue;
            }
            
            foreach (var item in events)
            {
                if (item.Start is null)
                {
                    logger.LogWarning("Start of event is null: {name}", item.Name);
                    continue;
                }
                
                // Calculate occurrences from today up to DaysAhead in the future
                // Using TakeWhile to prevent generating infinite occurrences for unbounded recurrence rules
                var startDate = new CalDateTime(DateTime.Now.Date.ToUniversalTime(), false);
                var endDate = DateTime.Now.Date.AddDays(sourceCalendars.DaysAhead).ToUniversalTime();
                var occurrences = item.GetOccurrences(startDate, null)
                    .TakeWhile(o => o.Period.StartTime.Value <= endDate)
                    .ToList();
                
                if (occurrences.Count > 1)
                {
                    foreach (var occurrence in occurrences)
                    {
                        AddEvent(item.Summary ?? string.Empty, occurrence.Period.StartTime.Value, occurrence.Period.EffectiveEndTime?.Value ?? occurrence.Period.StartTime.Value.AddHours(1), item.IsAllDay, item.Location, currentCalendar);
                    }
                }
                else if ((item.End is not null && item.End.Value.Date >= DateTime.Now.Date) || item.Start.Value.Date.AddDays(1) >= DateTime.Now.Date)
                {
                    AddEvent(item.Summary ?? string.Empty, item.Start.Value, item.End?.Value ?? item.Start.Value.Date.AddDays(1), item.IsAllDay, item.Location, currentCalendar);
                }
            }
        }

        IsLoading = false;
        RefreshedCalendars?.Invoke(this, EventArgs.Empty);
        logger.LogInformation("Finished calendar loading");
    }

    private void AddEvent(string eventName, DateTime start, DateTime end, bool isAllDay, string? eventLocation, Calendar currentCalendar)
    {
        CalendarEvent item = isAllDay
            ? new AllDayCalendarEvent(currentCalendar, eventName, 
                DateOnly.FromDateTime(start.Date),
                DateOnly.FromDateTime(end.Date.Subtract(TimeSpan.FromHours(12))), eventLocation)
            : new DefaultCalendarEvent(currentCalendar, eventName, start, end, eventLocation);
        Events.Add(item);
        logger.LogDebug("Added event: {json}", JsonConvert.SerializeObject(item));
    }
}
