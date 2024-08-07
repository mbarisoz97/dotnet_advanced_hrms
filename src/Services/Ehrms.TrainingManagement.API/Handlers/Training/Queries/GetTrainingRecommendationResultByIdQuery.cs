namespace Ehrms.TrainingManagement.API.Handlers.Training.Queries;

internal sealed class GetTrainingRecommendationResultByIdQuery : IRequest<TrainingRecommendationResult>
{
    public Guid Id { get; set; }
}

internal sealed class
    GetTrainingRecommendationResultByIdQueryHandler : IRequestHandler<GetTrainingRecommendationResultByIdQuery,
    TrainingRecommendationResult>
{
    private readonly TrainingDbContext _dbContext;

    public GetTrainingRecommendationResultByIdQueryHandler(TrainingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TrainingRecommendationResult> Handle(GetTrainingRecommendationResultByIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _dbContext.RecommendationResults
                         .Include(x => x.Employees)
                         .Include(x => x.Skill)
                         .Include(x => x.RecommendationRequest)
                         .FirstOrDefaultAsync(x => x.RecommendationRequest!.Id == request.Id, cancellationToken)
                     ?? throw new Exception("RESULT NOT FOUND!");

        return result;
    }
}