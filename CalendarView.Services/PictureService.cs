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
    public string? CurrentPictureUrl => CurrentItem;

    protected override IEnumerable<string> GetItems(IEnumerable<string> files)
    {
        var fileArray = files as string[] ?? files.ToArray();
        return fileArray.Where(IsImage)
            .Select(f =>
                $"data:{MimeMapping.MimeUtility.GetMimeMapping(f)};base64,{Convert.ToBase64String(File.ReadAllBytes(f))}")
            .Concat(fileArray.Where(IsVideo).Select(f =>
                $"data:{MimeMapping.MimeUtility.GetMimeMapping(f)};base64,{Convert.ToBase64String(File.ReadAllBytes(f))}"));

    }

    private static bool IsImage(string filePath)
    {
        return MimeMapping.MimeUtility.GetMimeMapping(filePath).StartsWith("image/");
    }

    private static bool IsVideo(string filePath)
    {
        var mimeMapping = MimeMapping.MimeUtility.GetMimeMapping(filePath);
        var fileExtension = Path.GetExtension(filePath).ToLowerInvariant();
        return mimeMapping.StartsWith("video/") || fileExtension == ".mp4" || fileExtension == ".mkv";
    }
}
