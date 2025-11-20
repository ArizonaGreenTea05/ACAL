using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CalendarView.Services;

public class PictureService
{
    public event EventHandler? PictureHasChanged;

    private readonly string _pictureDirectory;
    private readonly ILogger<PictureService> _logger;
    private Timer? _refreshTimer;
    private readonly List<string> _pictureBase64s = [];
    private int _currentPictureIndex = 0;

    public string? CurrentPictureBase64 => _currentPictureIndex >= _pictureBase64s.Count ? null : _pictureBase64s[_currentPictureIndex];

    public PictureService([FromKeyedServices("PictureRefreshInterval")] Common.TimeSpan refreshInterval, [FromKeyedServices("PictureDirectory")] string pictureDirectory, ILogger<PictureService> logger)
    {
        _pictureDirectory = pictureDirectory;
        _logger = logger;
        _refreshTimer?.Change(Timeout.Infinite, Timeout.Infinite);
        _refreshTimer = new Timer(RefreshTimerCallback, null, TimeSpan.Zero, refreshInterval.Value);
        _logger.LogInformation("Refresh timer started");
    }

    private void RefreshTimerCallback(object? state)
    {
        _logger.LogInformation("Refresh timer executing");
        Task.Run(UpdateCurrentPicture);
    }

    private void UpdateCurrentPicture()
    {
        if (_pictureBase64s.Count <= 0) LoadPictures();
        if (_pictureBase64s.Count <= 0) return;
        _currentPictureIndex = (_currentPictureIndex + 1) % _pictureBase64s.Count;
        _logger.LogDebug("Picture updated");
        PictureHasChanged?.Invoke(this, EventArgs.Empty);
    }


    private void LoadPictures()
    {
        _logger.LogInformation("Started loading pictures");
        var tempBase64s = new List<string>();
        if (!Directory.Exists(_pictureDirectory))
        {
            _logger.LogWarning("Picture directory does not exist: {path}", _pictureDirectory);
            return;
        }
        tempBase64s.AddRange(GetImageBase64s(Directory.EnumerateFiles(_pictureDirectory)));

        var subDirectories = Directory.EnumerateDirectories(_pictureDirectory).ToList();
        var matches = subDirectories.Where(d => MatchesCurrentDate(Path.GetFileName(d))).ToList();
        if (matches.Count <= 0)
        {
            var defaultImages = subDirectories.FirstOrDefault(d => Path.GetFileName(d) == "default");
            if (defaultImages is not null) matches.Add(defaultImages);
        }

        foreach (var match in matches)
        {
            tempBase64s.AddRange(GetImageBase64s(Directory.EnumerateFiles(match)));
        }

        _pictureBase64s.Clear();
        var random = new Random();
        _pictureBase64s.AddRange(tempBase64s.OrderBy(_ => random.Next()));

        _logger.LogInformation("Finished loading pictures");
    }

    private static bool MatchesCurrentDate(string folderName)
    {
        var dateStrings = folderName.Split('-', StringSplitOptions.RemoveEmptyEntries);

        var firstDate = GetDate(dateStrings[0]);
        if (firstDate is null) return false;
        var firstDateAsDateTime = new DateTime(firstDate.Value.year ?? DateTime.Today.Year,
            firstDate.Value.month, firstDate.Value.day);
        if (dateStrings.Length <= 1) return DateTime.Today == firstDateAsDateTime;
        var secondDate = GetDate(dateStrings[1]);
        if (secondDate is null) return false;
        var secondDateAsDateTime = new DateTime(secondDate.Value.year ?? DateTime.Today.Year,
            secondDate.Value.month, secondDate.Value.day);

        var options = new List<(DateTime start, DateTime end)>();

        if (firstDateAsDateTime < secondDateAsDateTime)
        {
            options.Add((firstDateAsDateTime, secondDateAsDateTime));
        }
        else
        {
            options.Add((new DateTime(firstDateAsDateTime.Year - 1, firstDateAsDateTime.Month, firstDateAsDateTime.Day), secondDateAsDateTime));
            options.Add((firstDateAsDateTime, new DateTime(secondDateAsDateTime.Year + 1, secondDateAsDateTime.Month, secondDateAsDateTime.Day)));
        }

        return options.Any(o => o.start <= DateTime.Now.Date && DateTime.Now.Date <= o.end);
    }

    private static (int day, int month, int? year)? GetDate(string dateString)
    {
        var split = dateString.Split('.');
        if (split.Length < 2
            || !int.TryParse(split[0], out var day)
            || !int.TryParse(split[1], out var month)) return null;
        if (split.Length < 3
            || !int.TryParse(split[2], out var year)) return (day, month, null);
        return (day, month, year);
    }

    private static IEnumerable<string> GetImageBase64s(IEnumerable<string> files)
    {
        return files.Where(f => MimeMapping.MimeUtility.GetMimeMapping(f).StartsWith("image/"))
            .Select(f => Convert.ToBase64String(File.ReadAllBytes(f)));
    }
}
