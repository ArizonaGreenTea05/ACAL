using CalendarView.Shared.Models;
using System.Net;
using System.Text;

namespace CalendarView.Web.Middleware;

public class BasicAuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly AuthenticationConfig _authConfig;
    private readonly ILogger<BasicAuthenticationMiddleware> _logger;

    public BasicAuthenticationMiddleware(
        RequestDelegate next,
        AuthenticationConfig authConfig,
        ILogger<BasicAuthenticationMiddleware> logger)
    {
        _next = next;
        _authConfig = authConfig;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Skip authentication if not enabled
        if (!_authConfig.Enabled)
        {
            await _next(context);
            return;
        }

        // Check for Authorization header
        if (!context.Request.Headers.ContainsKey("Authorization"))
        {
            _logger.LogDebug("No Authorization header found, returning 401");
            await ReturnUnauthorizedResponse(context);
            return;
        }

        var authHeader = context.Request.Headers["Authorization"].ToString();

        // Validate Basic authentication scheme
        if (!authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogWarning("Invalid authentication scheme: {Scheme}", authHeader.Split(' ')[0]);
            await ReturnUnauthorizedResponse(context);
            return;
        }

        // Decode credentials
        var encodedCredentials = authHeader.Substring("Basic ".Length).Trim();
        string decodedCredentials;
        try
        {
            decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to decode credentials");
            await ReturnUnauthorizedResponse(context);
            return;
        }

        var credentials = decodedCredentials.Split(':', 2);
        if (credentials.Length != 2)
        {
            _logger.LogWarning("Invalid credentials format");
            await ReturnUnauthorizedResponse(context);
            return;
        }

        var username = credentials[0];
        var password = credentials[1];
        var safeUsername = username
            .Replace("\r", string.Empty)
            .Replace("\n", string.Empty);

        // Validate credentials
        if (username == _authConfig.Username && password == _authConfig.Password)
        {
            _logger.LogDebug("Authentication successful for user: {Username}", safeUsername);
            await _next(context);
        }
        else
        {
            _logger.LogWarning("Authentication failed for user: {Username}", safeUsername);
            await ReturnUnauthorizedResponse(context);
        }
    }

    private static async Task ReturnUnauthorizedResponse(HttpContext context)
    {
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        context.Response.Headers.WWWAuthenticate = "Basic realm=\"ACAL Calendar\", charset=\"UTF-8\"";
        await context.Response.WriteAsync("Unauthorized");
    }
}
