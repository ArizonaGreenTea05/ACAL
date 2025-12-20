using Microsoft.Extensions.Logging;
using static CalendarView.Services.Common;
using CommonTimeSpan = Common.TimeSpan;

namespace CalendarView.Services;

public abstract class RefreshService<TItem, TClass> where TItem : class
{
    public event EventHandler? ItemHasChanged;

    private readonly string _directory;
    private readonly ILogger<TClass> _logger;
    private readonly Timer _refreshTimer;
    private readonly List<TItem> _items = [];
    private int _currentItemIndex = 0;
    private readonly FileSystemWatcher? _fileSystemWatcher;
    private bool _filesChanged = true;
    private DateOnly? _lastReloadDate;

    public TItem? CurrentItem => _currentItemIndex >= _items.Count ? null : _items[_currentItemIndex];

    protected RefreshService(CommonTimeSpan refreshInterval, string directory, ILogger<TClass> logger)
    {
        _directory = directory;
        _logger = logger;
        _refreshTimer = new Timer(RefreshTimerCallback, null, TimeSpan.Zero, refreshInterval.Value);
        _logger.LogInformation("Refresh timer started");

        if (!Directory.Exists(_directory)) return;
        _fileSystemWatcher = new FileSystemWatcher(_directory)
        {
            NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size,
            IncludeSubdirectories = true,
            EnableRaisingEvents = true
        };

        _fileSystemWatcher.Changed += FileSystemWatcher_Changed;
        _fileSystemWatcher.Created += FileSystemWatcher_Changed;
        _fileSystemWatcher.Deleted += FileSystemWatcher_Changed;
        _fileSystemWatcher.Renamed += FileSystemWatcher_Changed;
    }

    private void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
    {
        _filesChanged = true;
    }

    private void RefreshTimerCallback(object? state)
    {
        _logger.LogInformation("Refresh timer executing");
        Task.Run(UpdateCurrentItem);
    }

    private void UpdateCurrentItem()
    {
        var latestItem = CurrentItem;
        var reloadedText = LoadTextIfChanged();
        if (_items.Count <= 0) return;
        if (reloadedText) _currentItemIndex = 0;
        for (var i = 0; latestItem == CurrentItem && i < 3; i++)
        {
            _currentItemIndex = (_currentItemIndex + 1) % _items.Count;
        }
        _logger.LogDebug("Item updated");
        ItemHasChanged?.Invoke(this, EventArgs.Empty);
    }

    private bool LoadTextIfChanged()
    {
        _logger.LogInformation("Started loading items");
        var today = DateOnly.FromDateTime(DateTime.Now);
        if (!_filesChanged && _lastReloadDate == today)
        {
            _logger.LogInformation("No changes found. Abort loading");
            return false;
        }
        var tempItems = new List<TItem>();
        if (!Directory.Exists(_directory))
        {
            _logger.LogWarning("Items directory does not exist: {path}", _directory);
            return false;
        }
        tempItems.AddRange(GetItems(Directory.EnumerateFiles(_directory)));

        var subDirectories = Directory.EnumerateDirectories(_directory).ToList();
        var matches = subDirectories.Where(d => MatchesCurrentDate(Path.GetFileName(d))).ToList();
        if (matches.Count <= 0)
        {
            var defaultItems = subDirectories.FirstOrDefault(d => Path.GetFileName(d) == "default");
            if (defaultItems is not null) matches.Add(defaultItems);
        }

        foreach (var match in matches)
        {
            tempItems.AddRange(GetItems(Directory.EnumerateFiles(match)));
        }

        _items.Clear();
        var random = new Random();
        _items.AddRange(tempItems.OrderBy(_ => random.Next()));

        _filesChanged = false;
        _lastReloadDate = today;
        _logger.LogInformation("Finished loading items");
        return true;
    }

    protected abstract IEnumerable<TItem> GetItems(IEnumerable<string> files);
}
