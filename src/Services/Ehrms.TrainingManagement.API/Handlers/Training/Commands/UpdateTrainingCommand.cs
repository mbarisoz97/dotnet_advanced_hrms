using Ehrms.TrainingManagement.API.Database.Context;

namespace Ehrms.TrainingManagement.API.Handlers.Training.Commands;

public sealed class UpdateTrainingCommand : IRequest<Database.Models.Training>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime PlannedAt { get; set; }
    public ICollection<Guid> Participants { get; set; } = [];
}

internal sealed class UpdateTrainingCommandHandler : IRequestHandler<UpdateTrainingCommand, Database.Models.Training>
{
    private readonly IMapper _mapper;
    private readonly TrainingDbContext _trainingDbContext;

    public UpdateTrainingCommandHandler(IMapper mapper, TrainingDbContext trainingDbContext)
    {
        _mapper = mapper;
        _trainingDbContext = trainingDbContext;
    }

    public async Task<Database.Models.Training> Handle(UpdateTrainingCommand request, CancellationToken cancellationToken)
    {
        var training = await _trainingDbContext.Trainings
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new TrainingNotFoundException($"Could not find training with id '{request.Id}'");

        _mapper.Map(request, training);

        training.Participants = await _trainingDbContext.Employees
            .AsNoTracking()
            .Where(x => request.Participants.Contains(x.Id))
            .ToListAsync(cancellationToken);

        _trainingDbContext.Update(training);
        await _trainingDbContext.SaveChangesAsync(cancellationToken);

        return training;
    }
}
