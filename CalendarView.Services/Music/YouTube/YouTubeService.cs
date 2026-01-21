using CalendarView.Services.Music.Interfaces;
using CalendarView.Services.Music.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Timers;
using Google.Apis.YouTube.v3;
using Google.Apis.Services;
using Google.Apis.Auth.OAuth2;
using SpotifyAPI.Web;
using Timer = System.Timers.Timer;

namespace CalendarView.Services.Music.YouTube;

public class YouTubeService : IMusicService
{
    public event EventHandler? SongChanged;

    public event EventHandler? PlayStateChanged;

    private readonly Timer _timer = new(TimeSpan.FromSeconds(5));
    private readonly Timer _loginRefreshTimer = new(TimeSpan.FromMinutes(30));

    private bool _isLoggedIn = false;
    private readonly string _appdataFolderPath;
    private readonly ILogger _logger;

    // Simulated playback state since YouTube doesn't provide real-time playback API
    private string? _currentVideoId = null;
    private DateTime? _playbackStartTime = null;
    private TimeSpan _lastKnownProgress = TimeSpan.Zero;

    public Enums.PlayState PlayState { get; private set; } = Enums.PlayState.Unspecified;

    public Track? CurrentTrack { get; private set; } = null;

    public bool IsRunning => _isLoggedIn;

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
        _loginRefreshTimer.Stop();
        _loginRefreshTimer.Elapsed += LoginRefreshTimer_Elapsed;
        _timer.Stop();
        _timer.Elapsed += Timer_Elapsed;
        _logger.LogDebug($"Initialized {nameof(YouTubeService)}");
    }

    private async void LoginRefreshTimer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        if (await Login()) return;
        _logger.LogError("Login refresh failed");
    }

    private async void Timer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        await RefreshData();
    }

    private async Task RefreshData()
    {
        _logger.LogDebug("Fetching YouTube data");
        if (!_isLoggedIn)
        {
            _logger.LogError("Service is not logged in");
            return;
        }

        try
        {
            // Since YouTube doesn't provide a real-time playback API like Spotify,
            // this is a simulated implementation that would need to be connected
            // to an actual player (browser extension, native app, etc.)
            
            // For now, simulate progress tracking if playback is active
            if (PlayState == Enums.PlayState.Playing && CurrentTrack != null)
            {
                UpdateSimulatedProgress();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception was thrown when trying to fetch YouTube data: {ex}", ex.Message);
        }
    }

    private void UpdateSimulatedProgress()
    {
        if (_playbackStartTime.HasValue && CurrentTrack != null)
        {
            var elapsedTime = DateTime.Now - _playbackStartTime.Value;
            var newProgress = _lastKnownProgress + elapsedTime;
            
            if (newProgress >= CurrentTrack.Duration)
            {
                // Video finished, reset
                PlayState = Enums.PlayState.Paused;
                PlayStateChanged?.Invoke(this, EventArgs.Empty);
                return;
            }

            var updatedTrack = new Track(CurrentTrack.Name)
            {
                Artists = CurrentTrack.Artists,
                Duration = CurrentTrack.Duration,
                Progress = newProgress,
                Cover = CurrentTrack.Cover
            };

            CurrentTrack = updatedTrack;
            SongChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public void SetCurrentVideo(string videoId, string title, string channelName, TimeSpan duration, string? thumbnailUrl = null)
    {
        _logger.LogDebug("Setting current video: {videoId}", videoId);
        
        _currentVideoId = videoId;
        _lastKnownProgress = TimeSpan.Zero;
        _playbackStartTime = null;

        Image? cover = null;
        if (!string.IsNullOrEmpty(thumbnailUrl))
        {
            // Create an Image object for compatibility with Track model
            cover = new Image { Url = thumbnailUrl };
        }

        var newTrack = new Track(title)
        {
            Artists = [new Artist(channelName)],
            Duration = duration,
            Progress = TimeSpan.Zero,
            Cover = cover
        };

        if (newTrack != CurrentTrack)
        {
            CurrentTrack = newTrack;
            SongChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public async Task<bool> StartService()
    {
        _logger.LogDebug("Starting YouTube service");
        if (!await Login()) return false;
        _timer.Start();
        _loginRefreshTimer.Start();
        return true;
    }

    private async Task<bool> Login()
    {
        if (YouTubeLoginData is null)
        {
            _logger.LogError($"{nameof(YouTubeLoginData)} is null");
            throw new ArgumentNullException(nameof(YouTubeLoginData));
        }

        try
        {
            // For YouTube, we would typically use OAuth 2.0 authentication
            // This is a simplified implementation - in production, you would need:
            // 1. OAuth 2.0 credentials from Google Cloud Console
            // 2. User consent flow
            // 3. Token storage and refresh
            
            // For now, mark the service as logged in to allow basic functionality
            _isLoggedIn = true;
            _logger.LogDebug("YouTube service login successful");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError("YouTube login failed: {ex}", ex.Message);
            return false;
        }
    }

    public async Task<bool> StopService()
    {
        _logger.LogDebug("Stopping YouTube service");
        _timer.Stop();
        _loginRefreshTimer.Stop();
        _isLoggedIn = false;
        _currentVideoId = null;
        _playbackStartTime = null;
        CurrentTrack = null;
        PlayState = Enums.PlayState.Unspecified;
        return true;
    }

    public async Task<bool> Play()
    {
        if (!IsRunning
            || LoginData is null
            || !LoginData.CanControlPlayback)
        {
            _logger.LogWarning("Cannot play: service not running or playback control not allowed");
            return false;
        }

        try
        {
            _logger.LogDebug("Play command executed");
            
            // Mark playback start time for progress tracking
            _playbackStartTime = DateTime.Now;
            
            var newPlayState = Enums.PlayState.Playing;
            if (newPlayState != PlayState)
            {
                PlayState = newPlayState;
                PlayStateChanged?.Invoke(this, EventArgs.Empty);
            }
            
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception was thrown when trying to play: {ex}", ex.Message);
            return false;
        }
    }

    public async Task<bool> Pause()
    {
        if (!IsRunning
            || LoginData is null
            || !LoginData.CanControlPlayback)
        {
            _logger.LogWarning("Cannot pause: service not running or playback control not allowed");
            return false;
        }

        try
        {
            _logger.LogDebug("Pause command executed");
            
            // Update last known progress when pausing
            if (_playbackStartTime.HasValue && CurrentTrack != null)
            {
                var elapsedTime = DateTime.Now - _playbackStartTime.Value;
                _lastKnownProgress = CurrentTrack.Progress + elapsedTime;
            }
            _playbackStartTime = null;
            
            var newPlayState = Enums.PlayState.Paused;
            if (newPlayState != PlayState)
            {
                PlayState = newPlayState;
                PlayStateChanged?.Invoke(this, EventArgs.Empty);
            }
            
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception was thrown when trying to pause: {ex}", ex.Message);
            return false;
        }
    }

    public async Task<bool> Next()
    {
        if (!IsRunning
            || LoginData is null
            || !LoginData.CanControlPlayback)
        {
            _logger.LogWarning("Cannot skip to next: service not running or playback control not allowed");
            return false;
        }

        try
        {
            _logger.LogDebug("Next command executed");
            
            // In a real implementation, this would trigger the next video in a playlist
            // For now, we'll just reset the current track
            CurrentTrack = null;
            _currentVideoId = null;
            _playbackStartTime = null;
            _lastKnownProgress = TimeSpan.Zero;
            
            SongChanged?.Invoke(this, EventArgs.Empty);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception was thrown when trying to skip next: {ex}", ex.Message);
            return false;
        }
    }

    public async Task<bool> Previous()
    {
        if (!IsRunning
            || LoginData is null
            || !LoginData.CanControlPlayback)
        {
            _logger.LogWarning("Cannot skip to previous: service not running or playback control not allowed");
            return false;
        }

        try
        {
            _logger.LogDebug("Previous command executed");
            
            // In a real implementation, this would trigger the previous video
            // For now, reset the current track to beginning
            if (CurrentTrack != null)
            {
                var resetTrack = new Track(CurrentTrack.Name)
                {
                    Artists = CurrentTrack.Artists,
                    Duration = CurrentTrack.Duration,
                    Progress = TimeSpan.Zero,
                    Cover = CurrentTrack.Cover
                };
                CurrentTrack = resetTrack;
                _lastKnownProgress = TimeSpan.Zero;
                _playbackStartTime = DateTime.Now;
                
                SongChanged?.Invoke(this, EventArgs.Empty);
            }
            
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception was thrown when trying to skip previous: {ex}", ex.Message);
            return false;
        }
    }
}
