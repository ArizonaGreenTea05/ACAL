namespace CalendarView.Shared.Models;

public class PageLayoutTranslation(bool showImage, bool showBackgroundImage)
{
    public static Dictionary<Enums.PageLayout, PageLayoutTranslation> Translations { get; } = new()
    {
        { Enums.PageLayout.Agenda, new PageLayoutTranslation(false, false) },
        { Enums.PageLayout.AgendaWithImage, new PageLayoutTranslation(true, false) },
        { Enums.PageLayout.AgendaWithBackground, new PageLayoutTranslation(false, true) },
        { Enums.PageLayout.AgendaWithImageAndBackground, new PageLayoutTranslation(true, true) },
        { Enums.PageLayout.Calendar, new PageLayoutTranslation(false, false) },
        { Enums.PageLayout.CalendarWithImage, new PageLayoutTranslation(true, false) },
        { Enums.PageLayout.CalendarWithBackground, new PageLayoutTranslation(false, true) },
        { Enums.PageLayout.CalendarWithImageAndBackground, new PageLayoutTranslation(true, true) },
    };

    public bool ShowImage { get; private set; } = showImage;

    public bool ShowBackgroundImage { get; private set; } = showBackgroundImage;
}
