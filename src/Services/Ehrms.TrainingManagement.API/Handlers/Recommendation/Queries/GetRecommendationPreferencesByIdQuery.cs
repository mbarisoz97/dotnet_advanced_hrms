using LanguageExt.Common;

namespace Ehrms.TrainingManagement.API.Handlers.Recommendation.Queries;

public class GetRecommendationPreferencesByIdQuery : IRequest<Result<TrainingRecommendationPreferences>>
{
    public Guid Id { get; set; }    
}

internal class GetRecommendationPreferencesByIdQueryHandler
    : IRequestHandler<GetRecommendationPreferencesByIdQuery, Result<TrainingRecommendationPreferences>>
{
    private readonly TrainingDbContext _dbContext;

    public GetRecommendationPreferencesByIdQueryHandler(TrainingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<TrainingRecommendationPreferences>> Handle(GetRecommendationPreferencesByIdQuery request, CancellationToken cancellationToken)
    {
        var preference = await _dbContext.TrainingRecommendationPreferences
            .AsNoTracking()
            .Include(x=>x.Project)
            .Include(x=>x.Title)
            .Include(x=>x.Skills)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x=>x.Id == request.Id, cancellationToken);

        if (preference == null)
        {
            return new Result<TrainingRecommendationPreferences>(new TrainingRecommendationPreferenceNotFoundException(
                    $"Could not find preference with id : <{request.Id}>"));
        }

        return preference;
    }
}