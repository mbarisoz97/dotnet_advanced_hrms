using Ehrms.Authentication.API.Database.Context;
using Ehrms.Authentication.API.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Ehrms.Authentication.API.Handlers.User.Queries;

public sealed class GetUserByIdQuery : IRequest<Result<Database.Models.User>>
{
    public Guid Id { get; set; }
}

internal sealed class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<Database.Models.User>>
{
    private readonly ApplicationUserDbContext _dbContext;

    public GetUserByIdQueryHandler(ApplicationUserDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Database.Models.User>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .Include(x=>x.Roles)
            .FirstOrDefaultAsync(x=>x.Id == request.Id, cancellationToken);

        if (user == null)
        {
            UserNotFoundException notFoundException = new($"Could not find user with id : <{request.Id}>");
            return new Result<Database.Models.User>(notFoundException);
        }

        return new Result<Database.Models.User>(user);
    }
}