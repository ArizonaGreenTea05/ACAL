using Common.UI.Extensions;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Common.UI.Tests.Extensions;

[TestClass]
public class DateTimeExtensionsTests
{
    [TestMethod]
    public void TestToShortShortDateString()
    {
        var dateTime = new DateTime(2020, 2, 1);

        Assert.AreEqual("01.02.", dateTime.ToShortDateString(null, new CultureInfo("de")));
        Assert.AreEqual("01.02.", DateOnly.FromDateTime(dateTime).ToShortDateString(null, new CultureInfo("de")));
    }

    [TestMethod]
    public void TestToLongDayString()
    {
        var dateTime = new DateTime(2025, 11, 18);

        Assert.AreEqual("Dienstag", dateTime.ToLongDayString(null, new CultureInfo("de")));
        Assert.AreEqual("Tuesday", dateTime.ToLongDayString(null, new CultureInfo("en")));
    }

    [TestMethod]
    public void TestToLongMonthString()
    {
        var dateTime = new DateTime(2025, 1, 18);

        Assert.AreEqual("Januar", dateTime.ToLongMonthString(null, new CultureInfo("de")));
        Assert.AreEqual("January", dateTime.ToLongMonthString(null, new CultureInfo("en")));
    }

    [TestMethod]
    public void TestToShortTimeString()
    {
        var dateTime = new DateTime(new DateOnly(2025, 1, 18), new TimeOnly(17, 8, 9, 10, 11));

        Assert.AreEqual("17:08", dateTime.ToShortTimeString(null, new CultureInfo("de")));
        Assert.AreEqual(Regex.Replace("5:08 PM", @"\s", ""),
            Regex.Replace(dateTime.ToShortTimeString(null, new CultureInfo("en")).Replace(" ", ""), @"\s", ""));
    }

    [TestMethod]
    public void TestToLongDateString()
    {
        var dateTime = new DateTime(2025, 1, 18);

        Assert.AreEqual("Samstag, 18. Januar 2025", dateTime.ToLongDateString(null, new CultureInfo("de")));
        Assert.AreEqual("Saturday, January 18, 2025", dateTime.ToLongDateString(null, new CultureInfo("en")));
    }
}
