using Microsoft.EntityFrameworkCore;

namespace Ehrms.TrainingManagement.API.Handlers.Training.Queries;

internal class GetTrainingRecommendationResultsQuery : IRequest<IQueryable<TrainingRecommendationResult>>
{
    public Guid RecommendationRequestId { get; set; }
}

internal class GetTrainingRecommendationResultsQueryHandler : IRequestHandler<GetTrainingRecommendationResultsQuery,
    IQueryable<TrainingRecommendationResult>>
{
    private readonly TrainingDbContext _dbContext;

    public GetTrainingRecommendationResultsQueryHandler(TrainingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<IQueryable<TrainingRecommendationResult>> Handle(GetTrainingRecommendationResultsQuery request,
        CancellationToken cancellationToken)
    {
        var trainingRecommendationRecords = _dbContext.RecommendationResults
                .Include(x => x.Skill)
                .Include(x => x.Employees)
                .Include(x => x.RecommendationRequest)
                .Where(x => x.RecommendationRequest!.Id == request.RecommendationRequestId);

        return Task.FromResult(trainingRecommendationRecords);
    }
}