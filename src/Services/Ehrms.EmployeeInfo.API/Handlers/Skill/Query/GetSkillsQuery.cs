using Ehrms.EmployeeInfo.API.Database.Context;

namespace Ehrms.EmployeeInfo.API.Handlers.Skill.Query;

internal sealed record GetSkillsQuery : IRequest<IQueryable<Database.Models.Skill>>{}

internal sealed class GetSkillsQueryHandler : IRequestHandler<GetSkillsQuery, IQueryable<Database.Models.Skill>>
{
    private readonly EmployeeInfoDbContext _dbContext;

    public GetSkillsQueryHandler(EmployeeInfoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IQueryable<Database.Models.Skill>> Handle(GetSkillsQuery request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(_dbContext.Skills.AsQueryable());
    }
}