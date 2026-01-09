using CalendarView.Core.Models;

namespace CalendarView.Core.Tests.Models;

[TestClass]
public sealed class AllDayCalendarEventTests
{
    [TestMethod]
    public void TestStartTime()
    {
        CalendarEvent calendarEvent = new AllDayCalendarEvent(new Calendar { Name = "Test" }, "test",
            new DateOnly(2000, 1, 1), new DateOnly(2000, 1, 2), "test loc");

        var allDayCalendarEvent = calendarEvent as AllDayCalendarEvent;

        Assert.AreEqual(calendarEvent.TotalStartTime, allDayCalendarEvent?.Start.ToDateTime(new TimeOnly(0, 0)));
    }

    [TestMethod]
    public void TestEndTime()
    {
        CalendarEvent calendarEvent = new AllDayCalendarEvent(new Calendar { Name = "Test" }, "test",
            new DateOnly(2000, 1, 1), new DateOnly(2000, 1, 2), "test loc");

        var allDayCalendarEvent = calendarEvent as AllDayCalendarEvent;

        Assert.AreEqual(calendarEvent.TotalEndTime, allDayCalendarEvent?.End.ToDateTime(new TimeOnly(23,59)));
    }
}
