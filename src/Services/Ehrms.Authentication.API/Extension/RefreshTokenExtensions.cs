using Ehrms.Authentication.API.Database.Models;
using Ehrms.Authentication.API.Models;

namespace Ehrms.Authentication.API.Extension;

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