using CommunityToolkit.Mvvm.ComponentModel;

namespace CalendarView.Core.Models;

public partial class Notification(Enums.NotificationKind kind, string message) : ObservableObject
{
    [ObservableProperty] private Enums.NotificationKind _kind = kind;
    [ObservableProperty] private string _message = message;
    [ObservableProperty] private DateTime _createdAt = DateTime.Now;
}
