using SpotifyAPI.Web;

namespace CalendarView.Services.Music.Models;

public class Track(string name)
{
    public string Name { get; set; } = name;
    public List<Artist> Artists { get; set; } = [];
    public TimeSpan Duration { get; set; } = TimeSpan.Zero;
    public TimeSpan Progress { get; set; } = TimeSpan.Zero;
    public Image? Cover { get; set; }

    public int ProgressInPercent => Duration.TotalMilliseconds == 0 ? 0 : Convert.ToInt32(Progress.TotalMilliseconds / Duration.TotalMilliseconds * 100);

    public static bool operator ==(Track? left, Track? right)
    {
        if (left is null && right is null) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    public static bool operator !=(Track? left, Track? right)
    {
        return !(left == right);
    }

    public override bool Equals(object? obj)
    {
        return obj is Track track && track.Equals(this);
    }

    protected bool Equals(Track other)
    {
        return other.Name == Name
               && other.Artists.All(a => Artists.Contains(a))
               && Duration == other.Duration
               && Progress == other.Progress;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Artists, Duration, Progress);
    }
}
