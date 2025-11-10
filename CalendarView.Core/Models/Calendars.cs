using CommunityToolkit.Mvvm.ComponentModel;

namespace CalendarView.Core.Models;

public partial class Calendars : ObservableObject
{
    [ObservableProperty] private Dictionary<string, CalendarCustomization> _definitions = [];
    [ObservableProperty] private int _refreshAfterMinutes = 10;
}
