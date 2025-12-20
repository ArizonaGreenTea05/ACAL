namespace CalendarView.Services;

internal static class Common
{
    internal static (int day, int month, int? year)? GetDate(string dateString)
    {
        var split = dateString.Split('.');
        if (split.Length < 2
            || !int.TryParse(split[0], out var day)
            || !int.TryParse(split[1], out var month)) return null;
        if (split.Length < 3
            || !int.TryParse(split[2], out var year)) return (day, month, null);
        return (day, month, year);
    }

    internal static bool MatchesCurrentDate(string folderName)
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
}
