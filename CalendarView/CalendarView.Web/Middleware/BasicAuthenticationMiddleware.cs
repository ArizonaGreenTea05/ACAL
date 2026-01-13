using CalendarView.Shared.Models;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace CalendarView.Web.Middleware;

public class BasicAuthenticationMiddleware(
    RequestDelegate next,
    AuthenticationConfig authConfig,
    ILogger<BasicAuthenticationMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        // Skip authentication if not enabled
        if (!authConfig.Enabled)
        {
            await next(context);
            return;
        }

        // Check if already authenticated via cookie
        if (context.User.Identity?.IsAuthenticated == true)
        {
            logger.LogDebug("User already authenticated via cookie");
            await next(context);
            return;
        }

        // Check for Authorization header
        if (!context.Request.Headers.TryGetValue("Authorization", out var header))
        {
            logger.LogDebug("No Authorization header found, returning 401");
            await ReturnUnauthorizedResponse(context);
            return;
        }

        var authHeader = header.ToString();

        // Validate Basic authentication scheme
        if (!authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
        {
            logger.LogWarning("Invalid authentication scheme: {Scheme}", authHeader.Split(' ')[0]);
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
            logger.LogWarning(ex, "Failed to decode credentials");
            await ReturnUnauthorizedResponse(context);
            return;
        }

        var credentials = decodedCredentials.Split(':', 2);
        if (credentials.Length != 2)
        {
            logger.LogWarning("Invalid credentials format");
            await ReturnUnauthorizedResponse(context);
            return;
        }

        var username = credentials[0];
        var password = credentials[1];
        var safeUsername = username
            .Replace("\r", string.Empty)
            .Replace("\n", string.Empty);

        // Validate credentials
        if (username == authConfig.Username && password == authConfig.Password)
        {
            logger.LogDebug("Authentication successful for user: {Username}", safeUsername);
            
            // Create claims identity and sign in with cookie authentication
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, safeUsername),
                new Claim(ClaimTypes.NameIdentifier, safeUsername)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await context.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
                });

            logger.LogInformation("User {Username} signed in with cookie authentication", safeUsername);
            await next(context);
        }
        else
        {
            logger.LogWarning("Authentication failed for user: {Username}", safeUsername);
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
