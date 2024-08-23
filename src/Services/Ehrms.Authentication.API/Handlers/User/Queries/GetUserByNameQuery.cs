using Ehrms.Authentication.API.Adapter;
using Ehrms.Authentication.API.Exceptions;

namespace Ehrms.Authentication.API.Handlers.User.Queries;

public sealed class GetUserByNameQuery : IRequest<Result<Database.Models.User>>
{
    public string Username { get; set; } = string.Empty;
}

internal sealed class GetUserByNameQueryHandler : IRequestHandler<GetUserByNameQuery, Result<Database.Models.User>>
{
    private readonly IUserManagerAdapter _userManagerAdapter;

    public GetUserByNameQueryHandler(IUserManagerAdapter userManagerAdapter)
    {
        _userManagerAdapter = userManagerAdapter;
    }

    public async Task<Result<Database.Models.User>> Handle(GetUserByNameQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManagerAdapter.FindByNameAsync(request.Username);
        if (user == null)
        {
            return new Result<Database.Models.User>(
                new UserNotFoundException($"Could not find user <{request.Username}>"));
        }

        return new Result<Database.Models.User>(user);
    }
}