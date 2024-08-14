using Microsoft.EntityFrameworkCore;
using Ehrms.Authentication.API.Adapter;
using Ehrms.Authentication.API.Exceptions;
using Ehrms.Authentication.API.Database.Context;

namespace Ehrms.Authentication.API.Handlers.User.Commands;

public sealed class UpdateUserCommand : IRequest<Result<Database.Models.User>>
{
    public Guid Id { get; set; }
    public bool IsActive { get; set; }
    public IEnumerable<string> Roles { get; set; } = [];
}

internal sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result<Database.Models.User>>
{
    private readonly IMapper _mapper;
    private readonly IUserManagerAdapter _userManagerAdapter;
    private readonly ApplicationUserDbContext _dbContext;

    public UpdateUserCommandHandler(IUserManagerAdapter userManagerAdapter, IMapper mapper, ApplicationUserDbContext dbContext)
    {
        _userManagerAdapter = userManagerAdapter;
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<Result<Database.Models.User>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .Include(x=>x.Roles)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (user == null)
        {
            return new Result<Database.Models.User>(new UserNotFoundException($"Could not find user with id : <{request.Id}>"));
        }

        _mapper.Map(request, user);

        var updateUserIdentityResult = await _userManagerAdapter.UpdateAsync(user);
        if (!updateUserIdentityResult.Succeeded)
        {
            return new Result<Database.Models.User>(new UserUpdateFailedException());
        }

        var existingUserRoles = user.Roles.Select(x => x.Name);
        if (existingUserRoles != null)
        {
            var removeUserRolesIdentityResult = await _userManagerAdapter.RemoveFromRolesAsync(user, existingUserRoles!);
            if (!removeUserRolesIdentityResult.Succeeded)
            {
                return new Result<Database.Models.User>(new UserUpdateFailedException());
            }
        }

        var updatedUserRoles = request.Roles.Select(x => x.ToString());
        var updateUserRolesIdentityResult = await _userManagerAdapter.AddToRolesAsync(user, updatedUserRoles);
        if (!updateUserRolesIdentityResult.Succeeded)
        {
            return new Result<Database.Models.User>(new UserUpdateFailedException());
        }

        return new Result<Database.Models.User>(user);
    }
}