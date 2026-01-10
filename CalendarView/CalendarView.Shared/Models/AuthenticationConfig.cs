namespace CalendarView.Shared.Models;

public class AuthenticationConfig
{
    public bool Enabled { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
