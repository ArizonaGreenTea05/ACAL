using CommunityToolkit.Mvvm.ComponentModel;

namespace CalendarView.Core.Models;

public partial class AllDayCalendarEvent : CalendarEvent
{
    [ObservableProperty] private DateOnly _start;
    [ObservableProperty] private DateOnly _end;

    public override DateTime TotalStartTime => Start.ToDateTime(TimeOnly.FromTimeSpan(TimeSpan.Zero));
    public override DateTime TotalEndTime => End.ToDateTime(TimeOnly.FromTimeSpan(TimeSpan.Zero)).AddDays(1).Subtract(TimeSpan.FromMinutes(1));

    public AllDayCalendarEvent(Calendar calendar, string name, DateOnly start, DateOnly end) : base(calendar, name)
    {
        Start = start;
        End = end;
    }
}
