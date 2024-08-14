
using Ehrms.Authentication.API.Database.Context;

namespace Ehrms.Authentication.API.Handlers.Role.Queries;

public sealed record GetUserRolesQuery : IRequest<IQueryable<Database.Models.Role>>;

internal sealed class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, IQueryable<Database.Models.Role>>
{
    private readonly ApplicationUserDbContext _dbContext;

    public GetUserRolesQueryHandler(ApplicationUserDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IQueryable<Database.Models.Role>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(_dbContext.Roles);
    }
}