namespace Ehrms.TrainingManagement.API.Handlers.Training.Queries;

public sealed record GetTrainingRecommendationsQuery : IRequest<IQueryable<TrainingRecommendationRequest>>;

internal sealed class GetTrainingRecommendationsQueryHandler : IRequestHandler<GetTrainingRecommendationsQuery,
    IQueryable<TrainingRecommendationRequest>>
{
    private readonly TrainingDbContext _dbContext;

    public GetTrainingRecommendationsQueryHandler(TrainingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IQueryable<TrainingRecommendationRequest>> Handle(GetTrainingRecommendationsQuery request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(_dbContext.RecommendationRequests.AsNoTracking());
    }
}