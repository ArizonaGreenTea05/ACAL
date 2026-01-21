using CalendarView.Services.Music.Interfaces;
using CalendarView.Services.Music.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CalendarView.Services.Music.YouTube;

public class YouTubeService : IMusicService
{
    public event EventHandler? SongChanged;

    public event EventHandler? PlayStateChanged;

    private readonly string _appdataFolderPath;
    private readonly ILogger _logger;

    public Enums.PlayState PlayState { get; private set; } = Enums.PlayState.Unspecified;

    public Track? CurrentTrack { get; private set; } = null;

    public bool IsRunning => true;

    public IMusicServiceLoginData? LoginData
    {
        get => YouTubeLoginData;
        set
        {
            _logger.LogDebug($"Setting {nameof(LoginData)}");
            if (value is not YouTubeServiceLoginData ytld)
            {
                _logger.LogError($"{nameof(LoginData)} is not {nameof(YouTubeServiceLoginData)}");
                throw new InvalidCastException($"{nameof(LoginData)} must be of type {nameof(YouTubeServiceLoginData)}");
            }
            YouTubeLoginData = ytld;
            _logger.LogDebug($"Set {nameof(LoginData)} to {{json}}", JsonConvert.SerializeObject(YouTubeLoginData));
        }
    }

    public YouTubeServiceLoginData? YouTubeLoginData { get; set; }

    public YouTubeService(IMusicServiceLoginData loginData, string appdataFolderPath, ILogger logger)
    {
        _logger = logger;
        LoginData = loginData;
        _appdataFolderPath = appdataFolderPath;
        _logger.LogDebug($"Initialized {nameof(YouTubeService)}");
    }

    public async Task<bool> StartService()
    {
        return true;
    }

    private async Task<bool> Login()
    {
        return true;

    }

    public async Task<bool> StopService()
    {
        return true;
    }
    public async Task<bool> Play()
    {
        return true;
    }

    public async Task<bool> Pause()
    {
        return true;
    }

    public async Task<bool> Next()
    {
        return true;
    }

    public async Task<bool> Previous()
    {
        return true;
    }
}
