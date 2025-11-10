using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CalendarView.Shared.Models;

public partial class Design : ObservableObject
{
    [ObservableProperty] private string _background = "#1e1e1e";
    [ObservableProperty] private string _foreColorName = "LightGray";
    [ObservableProperty] private double _eventCardDimmingRatio = 0.3;
    [ObservableProperty] private string _language = CultureInfo.CurrentCulture.Name;
    [ObservableProperty] private string? _pictureDirectory;
    [ObservableProperty] private string? _customBackgroundImageOverlay;
    [ObservableProperty] private double _changePictureAfterMinutes = 2;
}
