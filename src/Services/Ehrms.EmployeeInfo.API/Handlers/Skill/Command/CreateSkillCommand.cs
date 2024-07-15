using Ehrms.EmployeeInfo.API.Database.Context;

namespace Ehrms.EmployeeInfo.API.Handlers.Skill.Command;

public sealed class CreateSkillCommand : IRequest<Database.Models.Skill>
{
    public string Name { get; set; } = string.Empty;
}

internal sealed class CreateSkillCommandHandler : IRequestHandler<CreateSkillCommand, Database.Models.Skill>
{
    private readonly IMapper _mapper;
    private readonly EmployeeInfoDbContext _dbContext;

    public CreateSkillCommandHandler(IMapper mapper,EmployeeInfoDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<Database.Models.Skill> Handle(CreateSkillCommand request, CancellationToken cancellationToken)
    {
        if (IsSkillNameInUse(request.Name))
        {
            throw new SkillNameIsInUseException($"'{request.Name}' already in use.");
        }

        var skill = _mapper.Map<Database.Models.Skill>(request);
        _dbContext.Skills.Add(skill);
        await _dbContext.SaveChangesAsync();

        return skill;
    }

    private bool IsSkillNameInUse(string skill)
    {
        return _dbContext.Skills.Any(x => x.Name == skill);
    }
}
