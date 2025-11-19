using System.Drawing;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CalendarView.Core.Models;

public partial class Calendar : ObservableObject
{
    [ObservableProperty] private string? _name;
    [ObservableProperty] private Color _color;

    public static bool operator ==(Calendar left, Calendar right)
    {
        if (left is null) return right is null;
        return left.Equals(right);
    }

    public static bool operator !=(Calendar left, Calendar right) => !(left == right);

    public bool Equals(Calendar? obj)
    {
        if (obj is null) return false;
        return Name == obj.Name
            && Color == obj.Color;
    }
}
