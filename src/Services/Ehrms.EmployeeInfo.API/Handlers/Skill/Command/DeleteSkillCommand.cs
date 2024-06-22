namespace Ehrms.EmployeeInfo.API.Handlers.Skill.Command;

internal sealed class DeleteSkillCommand : IRequest
{
    public Guid Id { get; set; }
}

internal sealed class DeleteSkillCommandHandler : IRequestHandler<DeleteSkillCommand>
{
    private readonly EmployeeInfoDbContext _dbContext;

    public DeleteSkillCommandHandler(EmployeeInfoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(DeleteSkillCommand request, CancellationToken cancellationToken)
    {
        var skill = await _dbContext.Skills
            .FirstOrDefaultAsync(x => x.Id == request.Id) 
            ?? throw new ArgumentException($"Could not find employee skill with id : {request.Id}");

        _dbContext.Remove(skill);
        await _dbContext.SaveChangesAsync();
    }
}