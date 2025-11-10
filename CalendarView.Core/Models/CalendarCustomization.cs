using CommunityToolkit.Mvvm.ComponentModel;

namespace CalendarView.Core.Models;

public partial class CalendarCustomization : ObservableObject
{
    [ObservableProperty] private string? _color;
    [ObservableProperty] private string? _customName;
}
