using System.Globalization;
using System.Text.RegularExpressions;

namespace Common.UI.Extensions;

public static class DateTimeExtensions
{
    public static string ToShortShortDateString(this DateTime dateTime, CultureInfo? cultureInfo = null) =>
        DateOnly.FromDateTime(dateTime).ToShortShortDateString(cultureInfo);

    public static string ToShortShortDateString(this DateOnly dateOnly, CultureInfo? cultureInfo = null)
    {
        cultureInfo ??= CultureInfo.CurrentCulture;
        var pattern = cultureInfo.DateTimeFormat.ShortDatePattern;
        pattern = Regex.Replace(pattern, "[/\\- ]?y+[/\\-. ]?", "");
        return dateOnly.ToString(pattern, cultureInfo);
    }

    public static string ToLongDayString(this DateTime dateTime, CultureInfo? cultureInfo = null) => dateTime.ToString("dddd", cultureInfo);

    public static string ToShortTimeString(this DateTime dateTime, CultureInfo? cultureInfo = null) => dateTime.ToString("t", cultureInfo);

    public static string ToLongDateString(this DateTime dateTime, CultureInfo? cultureInfo = null) => dateTime.ToString("D", cultureInfo);
}
