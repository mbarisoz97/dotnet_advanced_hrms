namespace Ehrms.TrainingManagement.API.UnitTests.TestHelpers.Fakers.Models;

internal sealed class TrainingRecommendationResultFaker : Faker<TrainingRecommendationResult>
{
    public TrainingRecommendationResultFaker WithEmployees(ICollection<Employee> employees)
    {
        RuleFor(x => x.Employees, employees);
        return this;
    }

    public TrainingRecommendationResultFaker WithSkill(Skill skill)
    {
        RuleFor(x => x.Skill, skill);
        return this;
    }

    public TrainingRecommendationResultFaker WithRequest(TrainingRecommendationRequest request)
    {
        RuleFor(x => x.RecommendationRequest, request);
        return this;
    }
}