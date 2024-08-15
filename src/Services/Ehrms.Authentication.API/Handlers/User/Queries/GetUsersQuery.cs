using Ehrms.Authentication.API.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace Ehrms.Authentication.API.Handlers.User.Queries;

public record GetUsersQuery : IRequest<IQueryable<Database.Models.User>>;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IQueryable<Database.Models.User>>
{
    private readonly ApplicationUserDbContext _dbContext;

    public GetUsersQueryHandler(ApplicationUserDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IQueryable<Database.Models.User>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(
            _dbContext.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ((UserRole)ur).Role)
        );
    }
}