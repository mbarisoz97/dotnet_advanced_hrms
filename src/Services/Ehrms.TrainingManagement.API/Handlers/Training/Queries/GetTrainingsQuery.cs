namespace Ehrms.TrainingManagement.API.Handlers.Training.Queries;

internal sealed record GetTrainingsQuery : IRequest<IQueryable<Models.Training>>;

internal sealed class GetTrainingsQueryHandler : IRequestHandler<GetTrainingsQuery, IQueryable<Models.Training>>
{
    private readonly TrainingDbContext _dbContext;

    public GetTrainingsQueryHandler(TrainingDbContext context)
    {
        _dbContext = context;
    }

    public Task<IQueryable<Models.Training>> Handle(GetTrainingsQuery request, CancellationToken cancellationToken)
    {
        var trainings = _dbContext.Trainings.AsNoTracking();
        return Task.FromResult(trainings);
    }
}