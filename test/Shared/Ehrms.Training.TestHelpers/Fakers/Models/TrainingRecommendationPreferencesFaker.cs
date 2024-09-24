namespace Ehrms.Training.TestHelpers.Fakers.Models;

public sealed class TrainingRecommendationPreferencesFaker : Faker<TrainingRecommendationPreferences>
{
    public TrainingRecommendationPreferencesFaker WithProject(Project project)
    {
        RuleFor(x=>x.Project, project);
        return this;
    }

    public TrainingRecommendationPreferencesFaker WithTitle(Title title)
    {
        RuleFor(x => x.Title, title);
        return this;
    }

    public TrainingRecommendationPreferencesFaker WithSkills(ICollection<Skill> skills)
    {
        RuleFor(x => x.Skills, skills);
        return this;
    }
}