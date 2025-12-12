using Newtonsoft.Json;
using Spotify.Models;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;

namespace Spotify;

public static class Authentication
{
    public static class Login
    {
        public static async Task<SpotifyClient?> LoginAsync(SpotifyLoginData loginData)
        {
            var token = await GetAuthTokenAsync(loginData);
            return token?.AccessToken is null ? null : new SpotifyClient(token.AccessToken);
        }

        public static async Task<SpotifyClient?> LoginAsync(SpotifyDockerLoginData loginData, string appdataFolderPath)
        {
            var tokenFilePath = Path.Combine(appdataFolderPath, "spotify_token.json");
            var token = await LoadTokenAsync(tokenFilePath) ?? loginData.AuthToken;
            if (token is null) return null;

            if (!TokenExpired(token)) return new SpotifyClient(token.AccessToken);
            token = await RefreshTokenAsync(loginData, token);
            if (token is null) return null;

            await SaveTokenAsync(token, tokenFilePath);

            return new SpotifyClient(token.AccessToken);
        }
    }

    public static async Task<SpotifyToken?> GetAuthTokenAsync(SpotifyLoginData loginData)
    {
        if (loginData.ClientId is null
            || loginData.ClientSecret is null
            || loginData.CallbackUrl is null
            || loginData.CallbackPort is null) return null;

        var authCodeSource = new TaskCompletionSource<string>();
        var server = new EmbedIOAuthServer(new Uri(loginData.CallbackUrl), (int)loginData.CallbackPort);

        await server.Start();

        server.AuthorizationCodeReceived += async (sender, response) =>
        {
            authCodeSource.TrySetResult(response.Code);
            await server.Stop();
        };

        var request = new LoginRequest(server.BaseUri, loginData.ClientId, LoginRequest.ResponseType.Code)
        {
            Scope =
            [
                Scopes.UserReadCurrentlyPlaying,
                Scopes.UserReadPlaybackState
            ]
        };

        BrowserUtil.Open(request.ToUri());

        var code = await authCodeSource.Task;

        var response = await new OAuthClient().RequestToken(
            new AuthorizationCodeTokenRequest(
                loginData.ClientId,
                loginData.ClientSecret,
                code,
                new Uri(loginData.CallbackUrl)
            )
        );


        return new SpotifyToken
        {
            AccessToken = response.AccessToken,
            RefreshToken = response.RefreshToken,
            TokenType = response.TokenType,
            ExpiresIn = response.ExpiresIn,
            CreatedAt = DateTime.UtcNow,
        };
    }

    private static async Task<SpotifyToken?> RefreshTokenAsync(SpotifyDockerLoginData loginData, SpotifyToken old)
    {
        if (string.IsNullOrEmpty(old.RefreshToken)
            || string.IsNullOrEmpty(loginData.ClientId)
            || string.IsNullOrEmpty(loginData.ClientSecret)) return null;

        var oauth = new OAuthClient();

        try
        {
            var response = await oauth.RequestToken(
                new AuthorizationCodeRefreshRequest(loginData.ClientId, loginData.ClientSecret, old.RefreshToken)
            );

            return new SpotifyToken
            {
                AccessToken = response.AccessToken,
                RefreshToken = response.RefreshToken ?? old.RefreshToken,
                TokenType = response.TokenType,
                ExpiresIn = response.ExpiresIn,
                CreatedAt = DateTime.Now,
            };
        }
        catch
        {
            return null;
        }
    }

    private static bool TokenExpired(SpotifyToken token)
    {
        var expiresAt = token.CreatedAt.AddSeconds(token.ExpiresIn);
        return DateTimeOffset.UtcNow >= expiresAt.AddHours(-1);
    }

    private static async Task<SpotifyToken?> LoadTokenAsync(string tokenFilePath)
    {
        if (!File.Exists(tokenFilePath)) return null;

        var json = await File.ReadAllTextAsync(tokenFilePath);
        return JsonConvert.DeserializeObject<SpotifyToken>(json);
    }

    private static async Task SaveTokenAsync(SpotifyToken token, string tokenFilePath)
    {
        var dir = Path.GetDirectoryName(tokenFilePath);
        if (dir is null) return;
        if (!string.IsNullOrWhiteSpace(dir) && !Directory.Exists(dir)) Directory.CreateDirectory(dir);
        var json = JsonConvert.SerializeObject(token, Formatting.Indented);
        await File.WriteAllTextAsync(tokenFilePath, json);
    }
}
