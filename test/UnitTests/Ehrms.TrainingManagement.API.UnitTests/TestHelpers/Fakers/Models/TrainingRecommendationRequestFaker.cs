namespace Ehrms.TrainingManagement.API.UnitTests.TestHelpers.Fakers.Models;

internal sealed class TrainingRecommendationRequestFaker : Faker<TrainingRecommendationRequest>
{
    public TrainingRecommendationRequestFaker()
    {
        RuleFor(x => x.Id, f => f.Random.Guid());
        RuleFor(x => x.Title, f => f.Random.Words(4));
    }

    public TrainingRecommendationRequestFaker WithProject(Project project)
    {
        RuleFor(x => x.ProjectId, project.Id);
        RuleFor(x => x.Project, project);
        return this;
    }

    public TrainingRecommendationRequestFaker WithRequestStatus(RequestStatus status)
    {
        RuleFor(x => x.RequestStatus, status);
        return this;
    }

    public TrainingRecommendationRequestFaker WithTrainingRecommendations(ICollection<TrainingRecommendationResult> trainingRecommendationResults)
    {
        RuleFor(x => x.TrainingRecommendation, trainingRecommendationResults);
        return this;
    }
}