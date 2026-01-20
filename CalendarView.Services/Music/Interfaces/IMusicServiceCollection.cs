using Microsoft.Extensions.Logging;

namespace CalendarView.Services.Music.Interfaces;

public interface IMusicServiceCollection
{
    List<IMusicService> Items { get; }

    void Add<T>(IMusicServiceLoginData loginData) where T : IMusicService;

    void Add(IMusicServiceLoginDataCollection loginData);

    Task<bool> StartServices();

    Task<bool> StopServices();
}
