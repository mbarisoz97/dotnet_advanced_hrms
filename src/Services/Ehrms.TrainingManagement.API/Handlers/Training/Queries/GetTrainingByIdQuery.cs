using Ehrms.TrainingManagement.API.Exceptions;

namespace Ehrms.TrainingManagement.API.Handlers.Training.Queries;

internal sealed class GetTrainingByIdQuery : IRequest<Models.Training>
{
    public Guid Id { get; set; }    
}

internal sealed class GetTrainingByIdQueryHandler : IRequestHandler<GetTrainingByIdQuery, Models.Training>
{
    private readonly TrainingDbContext _dbContext;

    public GetTrainingByIdQueryHandler(TrainingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Models.Training> Handle(GetTrainingByIdQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.Trainings.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken) 
            ?? throw new TrainingNotFoundException($"Could not find training with id '{request.Id}'");
    }
}