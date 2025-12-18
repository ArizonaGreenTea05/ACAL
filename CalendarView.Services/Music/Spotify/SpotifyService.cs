using CalendarView.Services.Music.Interfaces;
using CalendarView.Services.Music.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SpotifyAPI.Web;
using System.Timers;
using Newtonsoft.Json;
using Spotify;
using Timer = System.Timers.Timer;

namespace CalendarView.Services.Music.Spotify;

public class SpotifyService : IMusicService
{
    public event EventHandler? SongChanged;

    public event EventHandler? PlayStateChanged;

    private readonly Timer _timer = new(TimeSpan.FromSeconds(5));

    private readonly Timer _loginRefreshTimer = new(TimeSpan.FromMinutes(30));

    private SpotifyClient? _spotifyClient = null;
    private readonly string _appdataFolderPath;
    private readonly ILogger<SpotifyService> _logger;

    public Enums.PlayState PlayState { get; private set; } = Enums.PlayState.Unspecified;

    public Track? CurrentTrack { get; private set; } = null;

    public bool IsRunning => _spotifyClient is not null;

    public IMusicServiceLoginData? LoginData
    {
        get => SpotifyLoginData;
        set
        {
            _logger.LogDebug($"Setting {nameof(LoginData)}");
            if (value is not SpotifyServiceLoginData sld)
            {
                _logger.LogError($"{nameof(LoginData)} is not {nameof(SpotifyServiceLoginData)}");
                throw new InvalidCastException($"{nameof(LoginData)} must be of type {nameof(SpotifyServiceLoginData)}");
            }
            SpotifyLoginData = sld;
            _logger.LogDebug($"Set {nameof(LoginData)} to {{json}}", JsonConvert.SerializeObject(SpotifyLoginData));
        }
    }

    public SpotifyServiceLoginData? SpotifyLoginData { get; set; }

    public SpotifyService(IMusicServiceLoginData loginData, [FromKeyedServices("AppdataFolderPath")] string appdataFolderPath, ILogger<SpotifyService> logger)
    {
        _logger = logger;
        LoginData = loginData;
        _appdataFolderPath = appdataFolderPath;
        _loginRefreshTimer.Stop();
        _loginRefreshTimer.Elapsed += LoginRefreshTimer_Elapsed;
        _timer.Stop();
        _timer.Elapsed += Timer_Elapsed;
        _logger.LogDebug($"Initialized {nameof(SpotifyService)}");
    }

    private async void LoginRefreshTimer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        if (await Login()) return;
        _logger.LogError("Login refresh failed");
    }

    private async void Timer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        _logger.LogDebug("Fetching spotify data");
        if (_spotifyClient is null)
        {
            _logger.LogError($"{nameof(_spotifyClient)} is null");
            return;
        }

        try
        {
            var playback = (CurrentlyPlaying?)await _spotifyClient.Player.GetCurrentlyPlaying(new PlayerCurrentlyPlayingRequest());
            if (playback is null)
            {
                _logger.LogError($"Currently playing song is not {nameof(CurrentlyPlaying)}");
            }
            else
            {
                UpdatePlayState(playback);
            }
            UpdateTrack(playback);
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception was thrown when trying to fetch spotify data: {ex}", ex.Message);
        }
    }

    private void UpdateTrack(CurrentlyPlaying? playback)
    {
        _logger.LogDebug("Updating track");
        Track? newTrack = null;

        switch (playback?.Item)
        {
            case FullTrack track:
            {
                _logger.LogDebug("Playback item is a track");
                newTrack = new Track(track.Name)
                {
                    Artists = track.Artists.Select(a => new Artist(a.Name)).ToList(),
                    Duration = TimeSpan.FromMilliseconds(track.DurationMs),
                    Progress = TimeSpan.FromMilliseconds(playback.ProgressMs ?? 0),
                    Cover = track.Album.Images.FirstOrDefault()
                };
                break;
            }
            case FullEpisode episode:
            {
                _logger.LogDebug("Playback item is an episode");
                    newTrack = new Track(episode.Name)
                {
                    Artists = [new Artist(episode.Show.Name)],
                    Duration = TimeSpan.FromMilliseconds(episode.DurationMs),
                    Progress = TimeSpan.FromMilliseconds(playback.ProgressMs ?? 0),
                    Cover = episode.Images.FirstOrDefault()
                };
                break;
            }
            case null:
                _logger.LogError("Playback item is null");
                break;
            default:
                return;
        }

        if (newTrack == CurrentTrack) return;
        CurrentTrack = newTrack;
        SongChanged?.Invoke(this, EventArgs.Empty);
    }

    private void UpdatePlayState(CurrentlyPlaying playback)
    {
        _logger.LogDebug("Updating play state");
        var newPlayState = playback.IsPlaying ? Enums.PlayState.Playing : Enums.PlayState.Paused;
        if (newPlayState == PlayState) return;
        PlayState = newPlayState;
        PlayStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public async Task<bool> Start()
    {
        _logger.LogDebug("Starting spotify service");
        if (!await Login()) return false;
        _timer.Start();
        _loginRefreshTimer.Start();
        return true;
    }

    private async Task<bool> Login()
    {
        if (SpotifyLoginData is null)
        {
            _logger.LogError($"{nameof(SpotifyLoginData)} is null");
            throw new ArgumentNullException(nameof(SpotifyLoginData));
        }
        _spotifyClient = await Authentication.Login.LoginAsync(SpotifyLoginData, _appdataFolderPath, true, _logger);
        if (_spotifyClient is not null) return true;
        _logger.LogError($"{nameof(_spotifyClient)} is null");
        return false;

    }

    public async Task<bool> Stop()
    {
        _timer.Stop();
        _loginRefreshTimer.Stop();
        _spotifyClient = null;
        return true;
    }
}
