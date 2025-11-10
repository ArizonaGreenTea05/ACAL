using CommunityToolkit.Mvvm.ComponentModel;

namespace CalendarView.Shared.Models;

public partial class LoggingConfig : ObservableObject
{
    [ObservableProperty] private string _loggingTemplate = string.Empty;
    [ObservableProperty] private string _loggingPath = string.Empty;
    [ObservableProperty] private string _filteredLoggingPath = string.Empty;
}
