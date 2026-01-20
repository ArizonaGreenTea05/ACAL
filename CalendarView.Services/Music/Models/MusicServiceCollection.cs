using CalendarView.Services.Music.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swan.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace CalendarView.Services.Music.Models
{
    public class MusicServiceCollection : IMusicServiceCollection
    {
        private readonly string _appdataFolderPath;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger<MusicServiceCollection> _logger;

        public MusicServiceCollection(IMusicServiceLoginDataCollection musicServiceLoginData, [FromKeyedServices("AppdataFolderPath")] string appdataFolderPath, ILoggerFactory loggerFactory, ILogger<MusicServiceCollection> logger)
        {
            _appdataFolderPath = appdataFolderPath;
            _loggerFactory = loggerFactory;
            _logger = logger;
            Add(musicServiceLoginData);
        }

        public List<IMusicService> Items { get; } = [];

        public void Add(IMusicServiceLoginDataCollection loginData)
        {
            foreach (var item in loginData.LoginData)
            {
                var logger = _loggerFactory.CreateLogger(item.ServiceType);
                var type = logger.GetType();
                if (Activator.CreateInstance(item.ServiceType, item, _appdataFolderPath, logger) 
                    is not IMusicService musicService) continue;
                Items.Add(musicService);
            }
        }

        public void Add<T>(IMusicServiceLoginData loginData) where T : IMusicService
        {
            if (Activator.CreateInstance(typeof(T), loginData, _appdataFolderPath, _loggerFactory.CreateLogger<T>()) 
                is not IMusicService musicService) return;
            Items.Add(musicService);
        }

        public async Task<bool> StartServices()
        {
            foreach (var item in Items)
            {
                var success = await item.StartService();
                if (success) continue;
                return false;
            }
            return true;
        }

        public async Task<bool> StopServices()
        {
            foreach (var item in Items)
            {
                var success = await item.StopService();
                if (success) continue;
                return false;
            }
            return true;
        }
    }
}
