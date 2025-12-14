using CommunityToolkit.Mvvm.ComponentModel;
using System.Globalization;
using static CalendarView.Shared.Models.Enums;

namespace CalendarView.Shared.Models;

public partial class Design : ObservableObject
{
    [ObservableProperty] private DifferentiatedDesign _defaultDesign = new();
    [ObservableProperty] private Dictionary<PageLayout, DifferentiatedDesign> _designs = [];

    [ObservableProperty] private string _background = "#1e1e1e";
    [ObservableProperty] private string _foreColorName = "LightGray";
    [ObservableProperty] private bool _showScrollBar = true;
    [ObservableProperty] private double _eventCardDimmingRatio = 0.3;
    [ObservableProperty] private string _language = CultureInfo.CurrentCulture.Name;
    [ObservableProperty] private string? _longDateFormat;
    [ObservableProperty] private string? _shortDateFormat;
    [ObservableProperty] private string? _shortTimeFormat;
    [ObservableProperty] private string? _longMonthFormat;
    [ObservableProperty] private string? _longDayFormat;
    [ObservableProperty] private string? _pictureDirectory;
    [ObservableProperty] private double _changePictureAfterMinutes = 2;
    [ObservableProperty] private PageLayout _pageLayout;
    [ObservableProperty] private long? _dismissNotificationsAfterSeconds = 1800;
    [ObservableProperty] private int? _maxNotifications = null;

    public string? CustomSideImageOverlay
    {
        get => DefaultDesign.CustomSideImageOverlay;
        set => DefaultDesign.CustomSideImageOverlay = value;
    }

    public string? CustomBackgroundImageOverlay
    {
        get => DefaultDesign.CustomBackgroundImageOverlay;
        set => DefaultDesign.CustomBackgroundImageOverlay = value;
    }
    public string? CustomBackgroundImageBlur
    {
        get => DefaultDesign.CustomBackgroundImageBlur;
        set => DefaultDesign.CustomBackgroundImageBlur = value;
    }

    public bool SwapPictureAndContentInLandscape
    {
        get => DefaultDesign.SwapPictureAndContentInLandscape;
        set => DefaultDesign.SwapPictureAndContentInLandscape = value;
    }
    public bool SwapPictureAndContentInPortrait
    {
        get => DefaultDesign.SwapPictureAndContentInPortrait;
        set => DefaultDesign.SwapPictureAndContentInPortrait = value;
    }

    public bool ShowDate
    {
        get => DefaultDesign.ShowDate;
        set => DefaultDesign.ShowDate = value;
    }

    public bool ShowTime
    {
        get => DefaultDesign.ShowTime;
        set => DefaultDesign.ShowTime = value;
    }

    public bool ShowColorLegend
    {
        get => DefaultDesign.ShowColorLegend;
        set => DefaultDesign.ShowColorLegend = value;
    }

    public DifferentiatedDesign GetDesign(PageLayout? layout) => layout is not null && Designs.TryGetValue((PageLayout)layout, out var design) ? design : DefaultDesign;
}

public partial class DifferentiatedDesign : ObservableObject
{
    [ObservableProperty] private string? _customSideImageOverlay;
    [ObservableProperty] private string? _customBackgroundImageOverlay;
    [ObservableProperty] private string? _customBackgroundImageBlur;
    [ObservableProperty] private bool _swapPictureAndContentInLandscape;
    [ObservableProperty] private bool _swapPictureAndContentInPortrait;
    [ObservableProperty] private bool _showDate = true;
    [ObservableProperty] private bool _showTime = true;
    [ObservableProperty] private bool _showColorLegend = true;
}