namespace Ehrms.TrainingManagement.API.Handlers.Recommendation.Commands;

internal sealed class DeleteTrainingRecommendationRequestCommand : IRequest
{
    public Guid Id { get; set; }
}

internal sealed class DeleteTrainingRecommendationRequestCommandHandler : IRequestHandler<DeleteTrainingRecommendationRequestCommand>
{
    private readonly TrainingDbContext _dbContext;

    public DeleteTrainingRecommendationRequestCommandHandler(TrainingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(DeleteTrainingRecommendationRequestCommand request, CancellationToken cancellationToken)
    {
        var recommendationRequest = await _dbContext.RecommendationRequests.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
                                    ?? throw new TrainingRecommendationRequestNotFoundException($"Could not find recommendation request with id : <{request.Id}>");

        var recommendationResults = _dbContext.RecommendationResults
            .Include(x => x.RecommendationRequest)
            .Where(x => x.RecommendationRequest!.Id == request.Id);

        _dbContext.RemoveRange(recommendationResults);
        _dbContext.Remove(recommendationRequest);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}