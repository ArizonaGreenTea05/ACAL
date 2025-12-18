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
                Name = calendar.Value.CustomName
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
            }
            
            foreach (var item in events ?? [])
            {
                if (item.Start is null || item.End is null)
                {
                    logger.LogWarning("Start or end of event is null: {json}", JsonConvert.SerializeObject(item));
                    continue;
                }
                var occurrences = item.GetOccurrences(new CalDateTime(DateTime.Now.Date.ToUniversalTime(), false)).ToList();
                if (occurrences.Count > 1)
                {
                    foreach (var occurrence in occurrences)
                    {
                        AddEvent(item.Summary ?? string.Empty, occurrence.Period.StartTime.Value, occurrence.Period.EffectiveEndTime?.Value ?? occurrence.Period.StartTime.Value.AddHours(1), item.IsAllDay, currentCalendar);
                    }
                }
                else if (item.End.Value.Date >= DateTime.Now.Date)
                {
                    AddEvent(item.Summary ?? string.Empty, item.Start.Value, item.End.Value, item.IsAllDay, currentCalendar);
                }
            }
        }

        IsLoading = false;
        RefreshedCalendars?.Invoke(this, EventArgs.Empty);
        logger.LogInformation("Finished calendar loading");
    }

    private void AddEvent(string eventName, DateTime start, DateTime end, bool isAllDay, Calendar currentCalendar)
    {
        CalendarEvent item = isAllDay
            ? new AllDayCalendarEvent(currentCalendar, eventName, 
                DateOnly.FromDateTime(start.Date),
                DateOnly.FromDateTime(end.Date.Subtract(TimeSpan.FromHours(12))))
            : new DefaultCalendarEvent(currentCalendar, eventName, start, end);
        Events.Add(item);
        logger.LogDebug("Added event: {json}", JsonConvert.SerializeObject(item));
    }
}
