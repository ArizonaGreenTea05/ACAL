using CalendarView.Services.Music.Interfaces;
using CalendarView.Services.Music.Spotify;

namespace CalendarView.Services.Music.Models;

public class MusicServiceLoginDataCollection : IMusicServiceLoginDataCollection
{
    public List<IMusicServiceLoginData> LoginData => SpotifyLoginData.ConvertAll<IMusicServiceLoginData>(data => data);

    public List<SpotifyServiceLoginData> SpotifyLoginData { get; set; } = [];
}
