namespace Ehrms.Shared;

public sealed class AuthenticationRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}