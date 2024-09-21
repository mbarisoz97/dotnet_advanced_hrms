using LanguageExt.Common;

namespace Ehrms.TrainingManagement.API.Handlers.Recommendation.Commands;

public class UpdateTrainingRecommendationPreferenceCommand : IRequest<Result<TrainingRecommendationPreferences>>
{
    public Guid Id { get; set; }
    public ICollection<Guid> Skills { get; set; } = [];
}

internal class UpdateTrainingRecommendationPreferenceCommandHandler
    : IRequestHandler<UpdateTrainingRecommendationPreferenceCommand, Result<TrainingRecommendationPreferences>>
{
    private readonly TrainingDbContext _dbContext;

    public UpdateTrainingRecommendationPreferenceCommandHandler(TrainingDbContext trainingDbContext)
    {
        _dbContext = trainingDbContext;
    }

    public async Task<Result<TrainingRecommendationPreferences>> Handle(UpdateTrainingRecommendationPreferenceCommand request, CancellationToken cancellationToken)
    {
        var preference = await _dbContext.TrainingRecommendationPreferences
            .Include(x => x.Skills)
            .FirstOrDefaultAsync(x => x.Id == request.Id, default);

        if (preference == null)
        {
            return new Result<TrainingRecommendationPreferences>(new TrainingRecommendationPreferenceNotFoundException(
                    $"Could not find training preference with id : <{request.Id}>"));
        }

        var skills = _dbContext.Skills
            .Where(x => request.Skills.Contains(x.Id));
        if (skills.Count() != request.Skills.Count)
        {
            return new Result<TrainingRecommendationPreferences>(
                new SkillNotFoundException("Could not find all defined skills"));
        }

        await UpdatePreferenceSkills(request, preference);

        _dbContext.Update(preference);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return preference;
    }

    private async Task UpdatePreferenceSkills(UpdateTrainingRecommendationPreferenceCommand request, TrainingRecommendationPreferences preference)
    {
        var skillsToRemove = _dbContext.Skills
            .Where(x => !request.Skills.Contains(x.Id));

        await skillsToRemove.ForEachAsync(x => preference.Skills.Remove(x));

        var skillsToAdd = _dbContext.Skills
            .Where(x => request.Skills.Contains(x.Id) &&
                       !preference.Skills.Contains(x));

        await skillsToAdd.ForEachAsync(preference.Skills.Add);
    }
}
