namespace Ehrms.TrainingManagement.API.Handlers.Recommendation.Queries;

public record GetRecommendationPreferencesQuery : IRequest<IQueryable<TrainingRecommendationPreferences>>;

internal class GetRecommendationPreferencesQueryHandler
    : IRequestHandler<GetRecommendationPreferencesQuery, IQueryable<TrainingRecommendationPreferences>>
{
    private readonly TrainingDbContext _dbContext;

    public GetRecommendationPreferencesQueryHandler(TrainingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IQueryable<TrainingRecommendationPreferences>> Handle(GetRecommendationPreferencesQuery request, CancellationToken cancellationToken)
    {
        var preferences = _dbContext.TrainingRecommendationPreferences
            .Include(x => x.Project)
            .Include(x => x.Title)
            .Include(x => x.Skills)
            .AsSplitQuery()
            .AsNoTracking();

        return await Task.FromResult(preferences);
    }
}