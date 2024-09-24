using LanguageExt.Common;

namespace Ehrms.TrainingManagement.API.Handlers.Recommendation.Commands;

public class DeleteTrainingRecommendationPreferenceCommand : IRequest<Result<Guid>>
{
    public Guid Id { get; set; }
}

internal class DeleteTrainingRecommendationPreferenceCommandHandler :
    IRequestHandler<DeleteTrainingRecommendationPreferenceCommand, Result<Guid>>
{
    private readonly TrainingDbContext _dbContext;

    public DeleteTrainingRecommendationPreferenceCommandHandler(TrainingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Guid>> Handle(DeleteTrainingRecommendationPreferenceCommand request, CancellationToken cancellationToken)
    {
        var preference = await _dbContext.TrainingRecommendationPreferences
            .FirstOrDefaultAsync(x => x.Id == request.Id, default);

        if (preference == null)
        {
            return new Result<Guid>(new TrainingRecommendationPreferenceNotFoundException(
                    $"Could not find training preference with id : <{request.Id}>"));
        }

        _dbContext.Remove(preference);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return preference.Id;
    }
}