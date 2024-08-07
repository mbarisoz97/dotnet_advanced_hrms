namespace Ehrms.TrainingManagement.API.Handlers.Training.Queries;

internal sealed class GetTrainingRecommendationRequestByIdQuery : IRequest<TrainingRecommendationRequest>
{
    public Guid Id { get; set; }
}

internal sealed class
    GetTrainingRecommendationRequestByIdQueryHandler : IRequestHandler<GetTrainingRecommendationRequestByIdQuery,
    TrainingRecommendationRequest>
{
    private readonly TrainingDbContext _dbContext;

    public GetTrainingRecommendationRequestByIdQueryHandler(TrainingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TrainingRecommendationRequest> Handle(GetTrainingRecommendationRequestByIdQuery request,  CancellationToken cancellationToken)
    {
        var recommendationRequest = await _dbContext.RecommendationRequests.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
                                    ?? throw new TrainingRecommendationRequestNotFoundException($"Could not find training recommendation request with id : <{request.Id}>");

        return await Task.FromResult(recommendationRequest);
    }
}