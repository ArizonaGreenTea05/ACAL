namespace Spotify.Models;

public class SpotifyDockerLoginData
{
    public string? ClientId { get; set; }

    public string? ClientSecret { get; set; }

    public SpotifyToken? AuthToken { get; set; }
}
