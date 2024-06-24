using Ehrms.TrainingManagement.API.Exceptions;

namespace Ehrms.TrainingManagement.API.Handlers.Training.Commands;

internal sealed class DeleteTrainingCommand : IRequest
{
	public Guid Id { get; set; }
}

internal sealed class DeleteTrainingCommandHandler : IRequestHandler<DeleteTrainingCommand>
{
	private readonly TrainingDbContext _dbContext;

    public DeleteTrainingCommandHandler(TrainingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(DeleteTrainingCommand request, CancellationToken cancellationToken)
    {
        var training = await _dbContext.Trainings
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id)
            ?? throw new TrainingNotFoundException($"Could not find training with id '{request.Id}");

        _dbContext.Remove(training);
        await _dbContext.SaveChangesAsync(cancellationToken);    
    }
}