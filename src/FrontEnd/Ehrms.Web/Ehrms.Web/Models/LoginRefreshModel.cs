namespace Ehrms.Web.Models;

public class LoginRefreshModel
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}