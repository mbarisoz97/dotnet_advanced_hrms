
using Ehrms.EmployeeInfo.API.Database.Context;

namespace Ehrms.EmployeeInfo.API.Handlers.Skill.Query;

public sealed class GetSkillByIdQuery : IRequest<Database.Models.Skill>
{
    public Guid Id { get; set; }
}

internal sealed class GetSkillByIdQueryHandler : IRequestHandler<GetSkillByIdQuery, Database.Models.Skill>
{
    private readonly EmployeeInfoDbContext _dbContext;

    public GetSkillByIdQueryHandler(EmployeeInfoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Database.Models.Skill> Handle(GetSkillByIdQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.Skills
            .FirstOrDefaultAsync(x => x.Id == request.Id) 
            ?? throw new SkillNotFoundException($"Could not find an employee skill with id '{request.Id}'");
    }
}
