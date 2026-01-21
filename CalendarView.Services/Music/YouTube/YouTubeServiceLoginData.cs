using CalendarView.Services.Music.Interfaces;

namespace CalendarView.Services.Music.YouTube;

public class YouTubeServiceLoginData : IMusicServiceLoginData
{
    public bool CanControlPlayback { get; set; }
    public Type ServiceType => typeof(YouTubeService);
}
