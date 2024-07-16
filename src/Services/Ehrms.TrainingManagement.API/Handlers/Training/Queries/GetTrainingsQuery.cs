using Ehrms.TrainingManagement.API.Database.Context;

namespace Ehrms.TrainingManagement.API.Handlers.Training.Queries;

internal sealed record GetTrainingsQuery : IRequest<IQueryable<Database.Models.Training>>;

internal sealed class GetTrainingsQueryHandler : IRequestHandler<GetTrainingsQuery, IQueryable<Database.Models.Training>>
{
    private readonly TrainingDbContext _dbContext;

    public GetTrainingsQueryHandler(TrainingDbContext context)
    {
        _dbContext = context;
    }

    public Task<IQueryable<Database.Models.Training>> Handle(GetTrainingsQuery request, CancellationToken cancellationToken)
    {
        var trainings = _dbContext.Trainings.AsNoTracking();
        return Task.FromResult(trainings);
    }
}