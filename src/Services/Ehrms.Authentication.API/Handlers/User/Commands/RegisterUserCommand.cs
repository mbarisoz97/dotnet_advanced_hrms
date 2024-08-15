using Ehrms.Authentication.API.Adapter;
using Ehrms.Authentication.API.Database.Context;
using Ehrms.Authentication.API.Exceptions;

namespace Ehrms.Authentication.API.Handlers.User.Commands;

public enum UserRoles
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
    private readonly ApplicationUserDbContext _dbContext;
    private readonly IUserManagerAdapter _userManagerAdapter;

    public CreateUserCommandHandler(ApplicationUserDbContext dbContext, IUserManagerAdapter userManagerAdapter,
        IMapper mapper)
    {
        _mapper = mapper;
        _dbContext = dbContext;
        _userManagerAdapter = userManagerAdapter;
    }

    public async Task<Result<Database.Models.User>> Handle(RegisterUserCommand request,
        CancellationToken cancellationToken)
    {
        if (IsUserInformationInUse(request))
        {
            return new Result<Database.Models.User>(new UserDetailsInUseException());
        }

        var createdUser = _mapper.Map<Database.Models.User>(request);
        var identityResult = await _userManagerAdapter.CreateAsync(createdUser, request.Password);
        if (!identityResult.Succeeded)
        {
            return new Result<Database.Models.User>(new CouldNotCreateUserException());
        }

        await AddUserRoles(request, createdUser);
        return new Result<Database.Models.User>(createdUser);
    }

    private async Task AddUserRoles(RegisterUserCommand request, Database.Models.User createdUser)
    {
        var roles = _dbContext.Roles.Where(x => request.Roles.Contains(x.Name));
        foreach (var role in roles)
        {
            createdUser.UserRoles.Add(new UserRole()
            {
                User = createdUser,
                Role = role
            });
        }
        await _dbContext.SaveChangesAsync();
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