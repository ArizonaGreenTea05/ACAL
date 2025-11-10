using CommunityToolkit.Mvvm.ComponentModel;

namespace CalendarView.Core.Models;

public partial class DefaultCalendarEvent : CalendarEvent
{
    [ObservableProperty] private DateTime _start;
    [ObservableProperty] private DateTime _end;

    public override DateTime TotalStartTime => Start;
    public override DateTime TotalEndTime => End;

    public DefaultCalendarEvent(Calendar calendar, string name, DateTime start, DateTime end) : base(calendar, name)
    {
        Start = start;
        End = end;
    }
}
