using CommunityToolkit.Mvvm.ComponentModel;
using System.Globalization;
using static CalendarView.Shared.Models.Enums;

namespace CalendarView.Shared.Models;

public partial class Design : ObservableObject
{
    [ObservableProperty] private string _background = "#1e1e1e";
    [ObservableProperty] private string _foreColorName = "LightGray";
    [ObservableProperty] private double _eventCardDimmingRatio = 0.3;
    [ObservableProperty] private string _language = CultureInfo.CurrentCulture.Name;
    [ObservableProperty] private string? _longDateFormat;
    [ObservableProperty] private string? _shortDateFormat;
    [ObservableProperty] private string? _shortTimeFormat;
    [ObservableProperty] private string? _longMonthFormat;
    [ObservableProperty] private string? _longDayFormat;
    [ObservableProperty] private string? _pictureDirectory;
    [ObservableProperty] private string? _customSideImageOverlay;
    [ObservableProperty] private string? _customBackgroundImageOverlay;
    [ObservableProperty] private string? _customBackgroundImageBlur;
    [ObservableProperty] private double _changePictureAfterMinutes = 2;

    [ObservableProperty] private bool _swapPictureAndContentInLandscape;
    [ObservableProperty] private bool _swapPictureAndContentInPortrait;
    [ObservableProperty] private bool _showDate = true;
    [ObservableProperty] private bool _showTime = true;
    [ObservableProperty] private bool _showColorLegend = true;
    [ObservableProperty] private bool _showScrollBar = true;

    [ObservableProperty] private PageLayout _pageLayout;
}
