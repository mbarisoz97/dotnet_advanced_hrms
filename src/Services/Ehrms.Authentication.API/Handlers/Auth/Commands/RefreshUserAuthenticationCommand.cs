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

internal sealed class RefreshAuthenticationCommandHandler : IRequestHandler<RefreshAuthenticationCommand, Result<GenerateTokenResponse?>>
{
    private readonly ITokenHandler _tokenHandler;
    private readonly IUserManagerAdapter _userManagerAdapter;
    private readonly IMapper _mapper;

    public RefreshAuthenticationCommandHandler(ITokenHandler tokenHandler, IUserManagerAdapter userManagerAdapter, IMapper mapper)
    {
        _tokenHandler = tokenHandler;
        _userManagerAdapter = userManagerAdapter;
        _mapper = mapper;
    }

    public async Task<Result<GenerateTokenResponse?>> Handle(RefreshAuthenticationCommand request, CancellationToken cancellationToken)
    {
        var principal = _tokenHandler.GetPrincipalFromExpiredToken(request.AccessToken);
        var userName = principal?.Identity?.Name ?? string.Empty;
        if (string.IsNullOrEmpty(userName))
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

        if (!request.HasValidRefreshToken(user))
        {
            return new Result<GenerateTokenResponse?>(
                new InvalidTokenException("Refresh token is invalid"));
        }

        var authRequest = _mapper.Map<AuthenticationRequest>(request);
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