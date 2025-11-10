using CommunityToolkit.Mvvm.ComponentModel;

namespace CalendarView.Core.Models;

public abstract partial class CalendarEvent : ObservableObject
{
    [ObservableProperty] private Calendar _calendar;

    [ObservableProperty] private string? _name;

    [ObservableProperty] private bool _isReoccurring;

    protected CalendarEvent(Calendar calendar, string name)
    {
        Calendar = calendar;
        Name = name;
    }

    public abstract DateTime TotalStartTime { get; }
    public abstract DateTime TotalEndTime { get; }
}
