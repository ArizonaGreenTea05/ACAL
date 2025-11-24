using System.Globalization;
using System.Text.RegularExpressions;

namespace Common.UI.Extensions;

public static class DateTimeExtensions
{
    public static string ToShortDateString(this DateOnly dateOnly, string? overwriteFormat = null, CultureInfo? cultureInfo = null)
    {
        cultureInfo ??= CultureInfo.CurrentCulture;
        var pattern = cultureInfo.DateTimeFormat.ShortDatePattern;
        pattern = Regex.Replace(pattern, "[/\\- ]?y+[/\\-. ]?", "");
        return dateOnly.ToString(overwriteFormat ?? pattern, cultureInfo);
    }

    extension(DateTime dateTime)
    {
        public string ToShortDateString(string? overwriteFormat = null, CultureInfo? cultureInfo = null) =>
            DateOnly.FromDateTime(dateTime).ToShortDateString(overwriteFormat, cultureInfo);

        public string ToLongDayString(string? overwriteFormat = null, CultureInfo? cultureInfo = null) => dateTime.ToString(overwriteFormat ?? "dddd", cultureInfo);
        public string ToLongMonthString(string? overwriteFormat = null, CultureInfo? cultureInfo = null) => dateTime.ToString(overwriteFormat ?? "MMMM", cultureInfo);
        public string ToShortTimeString(string? overwriteFormat = null, CultureInfo? cultureInfo = null) => dateTime.ToString(overwriteFormat ?? "t", cultureInfo);
        public string ToLongDateString(string? overwriteFormat = null, CultureInfo? cultureInfo = null) => dateTime.ToString(overwriteFormat ?? "D", cultureInfo);
    }
}
