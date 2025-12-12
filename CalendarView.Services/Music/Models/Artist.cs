namespace CalendarView.Services.Music.Models;

public class Artist(string name)
{
    public string Name { get; set; } = name;

    public static bool operator ==(Artist? left, Artist? right)
    {
        if (left is null && right is null) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    public static bool operator !=(Artist? left, Artist? right)
    {
        return !(left == right);
    }

    public override bool Equals(object? obj)
    {
        return obj is Artist other && other.Equals(this);
    }

    protected bool Equals(Artist other)
    {
        return other.Name == Name;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name);
    }
}
