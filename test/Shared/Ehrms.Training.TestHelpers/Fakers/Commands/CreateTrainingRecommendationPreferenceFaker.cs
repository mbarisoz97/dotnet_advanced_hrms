using Ehrms.TrainingManagement.API.Handlers.Recommendation.Commands;

namespace Ehrms.Training.TestHelpers.Fakers.Commands;

public sealed class CreateTrainingRecommendationPreferenceFaker : Faker<CreateTrainingRecommendationPreferenceCommand>
{
    public CreateTrainingRecommendationPreferenceFaker()
    {
    }

    public CreateTrainingRecommendationPreferenceFaker WithProjectId(Guid id)
    {
        RuleFor(x => x.ProjectId, id);
        return this;
    }

    public CreateTrainingRecommendationPreferenceFaker WithTitleId(Guid id)
    {
        RuleFor(x => x.TitleId, id);
        return this;
    }

    public CreateTrainingRecommendationPreferenceFaker WithSkills(ICollection<Skill> skills)
    {
        RuleFor(x => x.Skills, skills.Select(x => x.Id).ToList());
        return this;
    }

    public CreateTrainingRecommendationPreferenceFaker WithSkills(ICollection<Guid> skills)
    {
        RuleFor(x => x.Skills, skills);
        return this;
    }
}
