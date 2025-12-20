using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using CommonTimeSpan = Common.TimeSpan;

namespace CalendarView.Services;

public class PictureService(
    [FromKeyedServices("PictureRefreshInterval")] CommonTimeSpan refreshInterval,
    [FromKeyedServices("PictureDirectory")] string pictureDirectory,
    ILogger<PictureService> logger)
    : RefreshService<string, PictureService>(refreshInterval, pictureDirectory, logger)
{
    public string? CurrentPictureBase64 => CurrentItem;

    protected override IEnumerable<string> GetItems(IEnumerable<string> files)
    {
        return files.Where(f => MimeMapping.MimeUtility.GetMimeMapping(f).StartsWith("image/"))
            .Select(f => Convert.ToBase64String(File.ReadAllBytes(f)));
    }
}
