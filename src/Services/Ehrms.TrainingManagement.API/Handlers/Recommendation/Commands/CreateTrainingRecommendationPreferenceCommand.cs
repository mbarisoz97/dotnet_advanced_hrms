using LanguageExt.Common;

namespace Ehrms.TrainingManagement.API.Handlers.Recommendation.Commands;

public class CreateTrainingRecommendationPreferenceCommand : IRequest<Result<TrainingRecommendationPreferences>>
{
    public Guid ProjectId { get; set; }
    public Guid TitleId { get; set; }
    public ICollection<Guid> Skills { get; set; } = [];
}

internal class CreateTrainingRecommendationPreferenceHandler
    : IRequestHandler<CreateTrainingRecommendationPreferenceCommand, Result<TrainingRecommendationPreferences>>
{
    private readonly TrainingDbContext _dbContext;

    public CreateTrainingRecommendationPreferenceHandler(TrainingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<TrainingRecommendationPreferences>> Handle(CreateTrainingRecommendationPreferenceCommand request, CancellationToken cancellationToken)
    {
        var project = await _dbContext.Projects.FirstOrDefaultAsync(x => x.Id == request.ProjectId, cancellationToken);
        if (project == null)
        {
            return new Result<TrainingRecommendationPreferences>(
                new ProjectNotFoundException($"Could not find project with id : {request.ProjectId}"));
        }

        var title = await _dbContext.Titles.FirstOrDefaultAsync(x => x.Id == request.TitleId, cancellationToken);
        if (title == null)
        {
            return new Result<TrainingRecommendationPreferences>(
                new TitleNotFoundException($"Could not find title with id : {request.TitleId}"));
        }

        var skills = _dbContext.Skills.Where(x => request.Skills.Contains(x.Id));
        if (skills.Length() != request.Skills.Length())
        {
            return new Result<TrainingRecommendationPreferences>(
                new SkillNotFoundException($"Could not find skills with id"));
        }

        TrainingRecommendationPreferences trainingRecommendationPreferences = new()
        {
            Project = project,
            Title = title
        };
        
        await skills.ForEachAsync(trainingRecommendationPreferences.Skills.Add, cancellationToken: cancellationToken);

        await _dbContext.AddAsync(trainingRecommendationPreferences, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return trainingRecommendationPreferences;
    }
}