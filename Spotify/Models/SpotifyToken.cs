namespace Spotify.Models;

public class SpotifyToken
{
    public string AccessToken { get; set; } = "";
    public string RefreshToken { get; set; } = "";
    public string TokenType { get; set; } = "Bearer";
    public int ExpiresIn { get; set; }
    public DateTime CreatedAt { get; set; }
}
