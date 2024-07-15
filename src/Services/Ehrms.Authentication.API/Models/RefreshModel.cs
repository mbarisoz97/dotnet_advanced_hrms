namespace Ehrms.Authentication.API.Models;

public sealed class RefreshModel
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}