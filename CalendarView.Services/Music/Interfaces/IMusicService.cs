using CalendarView.Services.Music.Models;

namespace CalendarView.Services.Music.Interfaces;

public interface IMusicService
{
    public event EventHandler? SongChanged;

    public event EventHandler? PlayStateChanged;

    public Enums.PlayState PlayState { get; }

    public Track? CurrentTrack { get; }

    public IMusicServiceLoginData? LoginData { get; set; }

    public bool IsRunning { get; }

    public Task<bool> Play();

    public Task<bool> Pause();

    public Task<bool> Next();

    public Task<bool> Previous();

    public Task<bool> StartService();

    public Task<bool> StopService();
}
