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
    public IEnumerable<string> Roles { get; set; } = [];
}

internal sealed class CreateUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<Database.Models.User>>
{
    private readonly IMapper _mapper;
    private readonly IUserManagerAdapter _userManagerAdapter;

    public CreateUserCommandHandler(IUserManagerAdapter userManagerAdapter, IMapper mapper)
    {
        _userManagerAdapter = userManagerAdapter;
        _mapper = mapper;
    }

    public async Task<Result<Database.Models.User>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (IsUserInformationInUse(request))
        {
            return new Result<Database.Models.User>(new UserDetailsInUseException());
        }

        var createdUser = _mapper.Map<Database.Models.User>(request);
        var identityResult = await _userManagerAdapter.CreateAsync(createdUser, request.Password);
        if (identityResult == null || !identityResult.Succeeded)
        {
            return new Result<Database.Models.User>(new CouldNotCreateUserException());
        }

        var userRoles = request.Roles.Select(x => x.ToString());
        var addRolesIdentityResult = await _userManagerAdapter.AddToRolesAsync(createdUser, userRoles);
        if (addRolesIdentityResult == null || !addRolesIdentityResult.Succeeded)
        {
            return new Result<Database.Models.User>(new CouldNotCreateUserException());
        }

        return new Result<Database.Models.User>(createdUser);
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