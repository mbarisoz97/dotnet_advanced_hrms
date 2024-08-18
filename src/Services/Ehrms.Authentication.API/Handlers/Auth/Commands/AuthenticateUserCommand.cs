using Ehrms.Shared;
using Ehrms.Authentication.API.Adapter;
using Ehrms.Authentication.API.Exceptions;
using System.Security.Cryptography;
using Ehrms.Authentication.API.Database.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace Ehrms.Authentication.API.Handlers.Auth.Commands;

public class AuthenticateUserCommand : IRequest<Result<GenerateTokenResponse?>>
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

internal sealed class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand, Result<GenerateTokenResponse?>>
{
    private readonly IMapper _mapper;
    private readonly ITokenHandler _tokenHandler;
    private readonly ApplicationUserDbContext _dbContext;
    private readonly IUserManagerAdapter _userManagerAdapter;

    public AuthenticateUserCommandHandler(ApplicationUserDbContext dbContext, IUserManagerAdapter userManagerAdapter, ITokenHandler tokenHandler, IMapper mapper)
    {
        _tokenHandler = tokenHandler;
        _mapper = mapper;
        _dbContext = dbContext;
        _userManagerAdapter = userManagerAdapter;
    }

    public async Task<Result<GenerateTokenResponse?>> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManagerAdapter.FindByNameAsync(request.Username);
        if (user == null)
        {
            return new Result<GenerateTokenResponse?>(new UserNotFoundException($"Could not find user <{request.Username}>"));
        }

        var isCredentialsValid = await _userManagerAdapter.CheckPasswordAsync(user, request.Password);
        if (!isCredentialsValid)
        {
            return new Result<GenerateTokenResponse?>(new UserCredentialsInvalidException($"User password is not correct."));
        }

        var authenticationRequest = _mapper.Map<AuthenticationRequest>(request);
        authenticationRequest.Roles = GetUserRoles(user.Id);

        var generateTokenResponse = _tokenHandler.Generate(authenticationRequest);
        if (generateTokenResponse != null)
        {
            generateTokenResponse!.RefreshToken = GenerateRefreshToken();
            user.RefreshToken = generateTokenResponse.RefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddMinutes(10);
            await _userManagerAdapter.UpdateAsync(user);
        }

        return new Result<GenerateTokenResponse?>(generateTokenResponse);
    }

    private IQueryable<string?> GetUserRoles(Guid userId)
    {
        return _dbContext.UserRoles
            .Include(x => x.Role)
            .Where(x => x.Role != null && x.UserId == userId)
            .Select(x => x.Role!.Name);
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var generator = RandomNumberGenerator.Create();
        generator.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}