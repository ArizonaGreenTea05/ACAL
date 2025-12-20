using CalendarView.Services.Text.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CommonTimeSpan = Common.TimeSpan;

namespace CalendarView.Services.Text;

public class TextService(
    [FromKeyedServices("TextRefreshInterval")] CommonTimeSpan refreshInterval,
    [FromKeyedServices("TextDirectory")] string textDirectory,
    ILogger<TextService> logger)
    : RefreshService<TextItem, TextService>(refreshInterval, textDirectory, logger)
{
    protected override IEnumerable<TextItem> GetItems(IEnumerable<string> files)
    {
        return files.Select(f => JsonConvert.DeserializeObject<TextGroup>(File.ReadAllText(f))).SelectMany(tg => tg is null ? [] : (List<TextItem>)tg).ToList();
    }
}
