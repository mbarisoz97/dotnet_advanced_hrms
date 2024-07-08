namespace Ehrms.Authentication.API.Controllers;

internal static class RefreshTokenExtensions
{
	internal static bool HasValidRefreshToken(this RefreshModel refreshModel, User user)
	{
		return user != null &&
			refreshModel != null &&
			user.RefreshToken == refreshModel.RefreshToken &&
			user.RefreshTokenExpiry > DateTime.UtcNow;
	}
}