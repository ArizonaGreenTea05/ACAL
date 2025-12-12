namespace Spotify.Models;

public class SpotifyLoginData
{
    public string? ClientId { get; set; }

    public string? ClientSecret { get; set; }

    public string? CallbackUrl { get; set; }

    public int? CallbackPort { get; set; }
}
