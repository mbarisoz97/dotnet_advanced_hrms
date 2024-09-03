using Ehrms.EmployeeInfo.API.Database.Context;
using Ehrms.EmployeeInfo.API.Exceptions.Title;

namespace Ehrms.EmployeeInfo.API.Handlers.Title.Query;

public sealed record GetTitleByIdQuery(Guid Id) : IRequest<Result<Database.Models.Title>>;

internal sealed class GetTitleByIdQueryHandler : IRequestHandler<GetTitleByIdQuery, Result<Database.Models.Title>>
{
    private readonly EmployeeInfoDbContext _dbContext;

    public GetTitleByIdQueryHandler(EmployeeInfoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Database.Models.Title>> Handle(GetTitleByIdQuery request, CancellationToken cancellationToken)
    {
        var title = await _dbContext.Titles.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (title == null)
        {
            return new Result<Database.Models.Title>(
                new TitleNotFoundException($"Could not find title with id : <{request.Id}>"));
        }

        return new Result<Database.Models.Title>(title);
    }
}