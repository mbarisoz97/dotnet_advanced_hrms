using Microsoft.EntityFrameworkCore;
using Ehrms.Authentication.API.Adapter;
using Ehrms.Authentication.API.Exceptions;
using Ehrms.Authentication.API.Database.Context;

namespace Ehrms.Authentication.API.Handlers.User.Commands;

public sealed class UpdateUserCommand : IRequest<Result<Database.Models.User>>
{
    public Guid Id { get; set; }
    public bool IsActive { get; set; }
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
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (user == null)
        {
            return new Result<Database.Models.User>(new UserNotFoundException($"Could not find user with id : <{request.Id}>"));
        }

        _mapper.Map(request, user);

        var result = await _userManagerAdapter.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return new Result<Database.Models.User>(new UserUpdateFailedException());
        }

        return new Result<Database.Models.User>(user);
    }
}