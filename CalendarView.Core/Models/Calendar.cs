using System.Drawing;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CalendarView.Core.Models;

public partial class Calendar : ObservableObject
{
    [ObservableProperty] private string? _name;
    [ObservableProperty] private Color _color;
}
