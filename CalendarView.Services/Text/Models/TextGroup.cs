namespace CalendarView.Services.Text.Models;

public class TextGroup
{
    public string Title { get; set; } = string.Empty;
    public List<TextItem> Items { get; set; } = [];


    public class TextItem
    {
        public string Text { get; set; } = string.Empty;
        public string AdditionalInfo { get; set; } = string.Empty;
    }

    public static explicit operator List<Models.TextItem>(TextGroup tg) => tg.Items.Select(i => new Models.TextItem
    {
        Title = tg.Title,
        Text = i.Text,
        AdditionalInfo = i.AdditionalInfo
    }).ToList();
}