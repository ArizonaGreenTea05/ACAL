using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;

namespace CalendarView.Shared.Models;

public class SingleLineTextFormatter(string outputTemplate) : ITextFormatter
{
    private readonly MessageTemplateTextFormatter _innerFormatter = new(outputTemplate, null);

    public void Format(LogEvent logEvent, TextWriter output)
    {
        using var sw = new StringWriter();
        _innerFormatter.Format(logEvent, sw);
        output.WriteLine(sw.ToString()
            .Replace("\r", "")
            .Replace("\n", "\t\\n\t"));
    }
}
