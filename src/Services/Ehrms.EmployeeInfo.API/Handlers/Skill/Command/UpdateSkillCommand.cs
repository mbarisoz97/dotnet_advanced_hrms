namespace Ehrms.EmployeeInfo.API.Handlers.Skill.Command;

public sealed class UpdateSkillCommand : IRequest<Models.Skill>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

internal sealed class UpdateSkillCommandHandler : IRequestHandler<UpdateSkillCommand, Models.Skill>
{
    private readonly IMapper _mapper;
    private readonly EmployeeInfoDbContext _dbContext;

    public UpdateSkillCommandHandler(IMapper mapper, EmployeeInfoDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<Models.Skill> Handle(UpdateSkillCommand request, CancellationToken cancellationToken)
    {
        var skill = await _dbContext.Skills
            .FirstOrDefaultAsync(x => x.Id == request.Id) 
            ?? throw new ArgumentException($"Could not find employee skill with id '{request.Id}'");

        _mapper.Map(request, skill);
        _dbContext.Skills.Update(skill);
        await _dbContext.SaveChangesAsync();

        return skill;
    }
}