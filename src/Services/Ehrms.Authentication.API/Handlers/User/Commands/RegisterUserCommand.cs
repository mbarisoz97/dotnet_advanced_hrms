using Ehrms.Authentication.API.Adapter;
using Ehrms.Authentication.API.Exceptions;

namespace Ehrms.Authentication.API.Handlers.User.Commands;

public enum UserRole
{
    User = 0,
    Manager,
    Admin,
}

public sealed class RegisterUserCommand : IRequest<Result<Database.Models.User>>
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

internal sealed class CreateUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<Database.Models.User>>
{
    private IUserManagerAdapter _userManagerAdapter;

    public CreateUserCommandHandler(IUserManagerAdapter userManagerAdapter)
    {
        _userManagerAdapter = userManagerAdapter;
    }

    public async Task<Result<Database.Models.User>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (IsUserInformationInUse(request))
        {
            return new Result<Database.Models.User>(new UserDetailsInUseException());
        }

        var user = new Database.Models.User
        {
            UserName = request.Username,
            Email = request.Email,
            IsActive = false
        };

        var identityResult = await _userManagerAdapter.CreateAsync(user, request.Password);
        if (identityResult == null || !identityResult.Succeeded)
        {
            return new Result<Database.Models.User>(new CouldNotCreateUserException());
        }

        return new Result<Database.Models.User>(user);
    }

    private bool IsUserInformationInUse(RegisterUserCommand request)
    {
        var users = _userManagerAdapter.Users.Where(x => x.UserName == request.Username ||
                x.NormalizedUserName == request.Username ||
                x.Email == request.Email ||
                x.NormalizedEmail == request.Email);

        return users.Any();
    }
}