using Ehrms.TrainingManagement.API.Handlers.Recommendation.Commands;

namespace Ehrms.Training.TestHelpers.Fakers.Commands;

public sealed class UpdateTrainingRecommendationPreferenceFaker : Faker<UpdateTrainingRecommendationPreferenceCommand>
{
    public UpdateTrainingRecommendationPreferenceFaker()
    {
    }

    public UpdateTrainingRecommendationPreferenceFaker WithId(Guid id)
    {
        RuleFor(x => x.Id, id);
        return this;
    }

    public UpdateTrainingRecommendationPreferenceFaker WithSkills(ICollection<Guid> skills)
    {
        RuleFor(x => x.Skills, skills);
        return this;
    }

    public UpdateTrainingRecommendationPreferenceFaker WithSkills(ICollection<Skill> skills)
    {
        RuleFor(x => x.Skills, skills.Select(x=>x.Id).ToList());
        return this;
    }
}

