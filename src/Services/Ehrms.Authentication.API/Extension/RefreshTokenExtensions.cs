using Ehrms.Authentication.API.Handlers.Auth.Commands;
using Ehrms.Authentication.API.Models;

namespace Ehrms.Authentication.API.Extension;

internal static class RefreshTokenExtensions
{
    internal static bool HasValidRefreshToken(this RefreshAuthenticationCommand command, User user)
    {
        return user != null &&
            command != null &&
            user.RefreshToken == command.RefreshToken &&
            user.RefreshTokenExpiry > DateTime.UtcNow;
    }
}