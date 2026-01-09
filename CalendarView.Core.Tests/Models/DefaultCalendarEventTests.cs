using CalendarView.Core.Models;

namespace CalendarView.Core.Tests.Models;

[TestClass]
public sealed class DefaultCalendarEventTests
{
    [TestMethod]
    public void TestStartTime()
    {
        CalendarEvent calendarEvent = new DefaultCalendarEvent(new Calendar { Name = "Test" }, "test",
            new DateTime(new DateOnly(2000, 1, 1), new TimeOnly(5, 6, 7, 8)),
            new DateTime(new DateOnly(2000, 1, 2), new TimeOnly(9, 8, 7, 6)), "test loc");

        var defaultCalendarEvent = calendarEvent as DefaultCalendarEvent;

        Assert.AreEqual(calendarEvent.TotalStartTime, defaultCalendarEvent?.Start);
    }

    [TestMethod]
    public void TestEndTime()
    {
        CalendarEvent calendarEvent = new DefaultCalendarEvent(new Calendar { Name = "Test" }, "test",
            new DateTime(new DateOnly(2000, 1, 1), new TimeOnly(5, 6, 7, 8)),
            new DateTime(new DateOnly(2000, 1, 2), new TimeOnly(9, 8, 7, 6)), "test loc");

        var defaultCalendarEvent = calendarEvent as DefaultCalendarEvent;

        Assert.AreEqual(calendarEvent.TotalEndTime, defaultCalendarEvent?.End);
    }
}
