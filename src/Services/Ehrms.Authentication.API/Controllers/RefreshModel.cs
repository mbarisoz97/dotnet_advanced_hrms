namespace Ehrms.Authentication.API.Controllers;

public sealed class RefreshModel
{
	public string AccessToken { get; set; } = string.Empty;
	public string RefreshToken { get; set; } = string.Empty;
}