namespace Ehrms.Web.Models;

public class LoginResponseModel
{
	public string AccessToken { get; set; } = string.Empty;
	public string RefreshToken { get; set; } = string.Empty;
}