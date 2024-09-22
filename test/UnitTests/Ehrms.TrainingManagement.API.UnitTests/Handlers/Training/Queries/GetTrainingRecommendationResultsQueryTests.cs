using Ehrms.TrainingManagement.API.UnitTests.Consumers.Training;

namespace Ehrms.TrainingManagement.API.UnitTests.Handlers.Training.Queries;

public class GetTrainingRecommendationResultsQueryTests : UnitTestsBase<TrainingDbContext>
{
    private readonly GetTrainingRecommendationResultsQueryHandler _handler;

    public GetTrainingRecommendationResultsQueryTests()
        : base(TestDbContextFactory.CreateDbContext(nameof(GetTrainingRecommendationResultsQueryTests)))
    {
        _handler = new(dbContext);
    }

    [Fact]
    public async Task Handle_NoRecommendations_ReturnsEmptyCollection()
    {
        var request = new TrainingRecommendationRequestFaker().Generate();
        await dbContext.RecommendationRequests.AddAsync(request);
        await dbContext.SaveChangesAsync();

        var query = new GetTrainingRecommendationResultsQuery() { RecommendationRequestId = request.Id };
        var trainingRecommendationResults = await _handler.Handle(query, default);

        trainingRecommendationResults.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ExistingRecommendations_ReturnsCreatedRecommendationResults()
    {
        var skills = new SkillFaker().Generate(3);
        await dbContext.Skills.AddRangeAsync(skills);

        var employees = new EmployeeFaker().Generate(2);
        await dbContext.AddRangeAsync(employees);

        var project = new ProjectFaker()
            .WithEmployees(employees)
            .WithRequiredSkills(skills)
            .Generate();
        await dbContext.AddRangeAsync(project);

        var request = new TrainingRecommendationRequestFaker()
            .WithProject(project)
            .Generate();
        await dbContext.AddRangeAsync(request);

        var trainingRecommendations = new TrainingRecommendationResultFaker()
            .WithEmployees(employees)
            .WithRequest(request)
            .Generate(2);
        await dbContext.AddRangeAsync(trainingRecommendations);
        await dbContext.SaveChangesAsync();

        var query = new GetTrainingRecommendationResultsQuery() { RecommendationRequestId = request.Id };
        var returnedRecommendationResults = await _handler.Handle(query, default);

        returnedRecommendationResults.Should().BeEquivalentTo(trainingRecommendations);
    }
}