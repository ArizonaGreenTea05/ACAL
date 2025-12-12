using Newtonsoft.Json;
using Spotify.Models;

namespace HelperProjects.SpotifyTokenHelper;

internal class Program
{
    private static async Task Main()
    {
        var loginData = JsonConvert.DeserializeObject<SpotifyLoginData>(await File.ReadAllTextAsync("SpotifyLoginData.json"));

        if (loginData is null || loginData.GetType().GetProperties().Where(p => p is { CanRead: true, CanWrite: true })
                .Any(p => p.GetValue(loginData) is null))
            throw new InvalidOperationException("Failed to load Spotify login data.");

        var token = await Spotify.Authentication.GetAuthTokenAsync(loginData);

        await File.WriteAllTextAsync("SpotifyAuthToken.txt", JsonConvert.SerializeObject(token, Formatting.Indented));
        Console.WriteLine("Token saved to file.");
    }
}