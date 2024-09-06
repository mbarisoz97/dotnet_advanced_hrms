using Ehrms.EmployeeInfo.API.Database.Context;

namespace Ehrms.EmployeeInfo.API.Handlers.Title.Query;

public sealed record GetAllTitlesQuery : IRequest<IQueryable<Database.Models.Title>>;

internal sealed class GetAllTitlesQueryHandler : IRequestHandler<GetAllTitlesQuery, IQueryable<Database.Models.Title>>
{
    private readonly EmployeeInfoDbContext _dbContext;

    public GetAllTitlesQueryHandler(EmployeeInfoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IQueryable<Database.Models.Title>> Handle(GetAllTitlesQuery request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(_dbContext.Titles.AsNoTracking());
    }
}