using CommunityToolkit.Mvvm.ComponentModel;

namespace CalendarView.Core.Models;

public partial class DefaultCalendarEvent : CalendarEvent
{
    [ObservableProperty] private DateTime _start;
    [ObservableProperty] private DateTime _end;

    public override DateTime TotalStartTime => Start;
    public override DateTime TotalEndTime => End;

    public DefaultCalendarEvent(Calendar calendar, string name, DateTime start, DateTime end, string? location) : base(calendar, name, location)
    {
        Start = start;
        End = end;
    }
}
