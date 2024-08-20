using System.Security.Claims;
using Ehrms.Shared;
using Ehrms.Authentication.API.Adapter;
using Ehrms.Authentication.API.Exceptions;
using Ehrms.Authentication.API.Extension;

namespace Ehrms.Authentication.API.Handlers.Auth.Commands;

public sealed class RefreshAuthenticationCommand : IRequest<Result<GenerateTokenResponse?>>
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}

internal sealed class
    RefreshAuthenticationCommandHandler : IRequestHandler<RefreshAuthenticationCommand, Result<GenerateTokenResponse?>>
{
    private readonly ITokenHandler _tokenHandler;
    private readonly IUserManagerAdapter _userManagerAdapter;

    public RefreshAuthenticationCommandHandler(ITokenHandler tokenHandler, IUserManagerAdapter userManagerAdapter)
    {
        _tokenHandler = tokenHandler;
        _userManagerAdapter = userManagerAdapter;
    }

    public async Task<Result<GenerateTokenResponse?>> Handle(RefreshAuthenticationCommand request,
        CancellationToken cancellationToken)
    {
        var claimsPrincipal = _tokenHandler.GetClaimsFromAccessToken(request.AccessToken);
        var userName = claimsPrincipal?.Identity?.Name ?? string.Empty;
        if (claimsPrincipal == null || string.IsNullOrEmpty(userName))
        {
            return new Result<GenerateTokenResponse?>(
                new InvalidTokenException("Access token is invalid"));
        }

        var user = await _userManagerAdapter.FindByNameAsync(userName);
        if (user == null)
        {
            return new Result<GenerateTokenResponse?>(
                new UserNotFoundException($"Could not find user with username {userName}>"));
        }

        if (!user.IsActive)
        {
            return new Result<GenerateTokenResponse?>(
                new UserAccountInactiveException("User account is inactive"));
        }

        if (!request.HasValidRefreshToken(user))
        {
            return new Result<GenerateTokenResponse?>(
                new InvalidTokenException("Refresh token is invalid"));
        }

        var userRoles = claimsPrincipal.FindAll(ClaimTypes.Role).Select(x => x.Value);
        var authRequest = new GenerateJwtRequest()
        {
            Username = userName,
            Roles = userRoles
        };
        var authenticationResponse = _tokenHandler.Generate(authRequest);
        if (authenticationResponse == null)
        {
            return new Result<GenerateTokenResponse?>(
                new InvalidTokenException("Could not create access token"));
        }
        authenticationResponse.RefreshToken = request.RefreshToken;

        return new Result<GenerateTokenResponse?>(authenticationResponse);
    }
}