using CommunityToolkit.Mvvm.ComponentModel;

namespace CalendarView.Core.Models;

public abstract partial class CalendarEvent : ObservableObject
{
    [ObservableProperty] private Calendar _calendar;

    [ObservableProperty] private string? _name;

    [ObservableProperty] private string? _location;

    [ObservableProperty] private bool _isReoccurring;

    protected CalendarEvent(Calendar calendar, string name, string? location)
    {
        Calendar = calendar;
        Name = name;
        Location = location;
    }

    public abstract DateTime TotalStartTime { get; }
    public abstract DateTime TotalEndTime { get; }
}
