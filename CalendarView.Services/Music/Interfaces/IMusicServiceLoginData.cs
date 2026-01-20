namespace CalendarView.Services.Music.Interfaces;

public interface IMusicServiceLoginData
{
    bool CanControlPlayback { get; }

    Type ServiceType { get; }
}
