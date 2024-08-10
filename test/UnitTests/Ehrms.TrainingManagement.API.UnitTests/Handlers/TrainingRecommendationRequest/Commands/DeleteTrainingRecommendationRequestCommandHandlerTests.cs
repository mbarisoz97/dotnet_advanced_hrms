using Ehrms.TrainingManagement.API.UnitTests.Consumers.Training;

namespace Ehrms.TrainingManagement.API.UnitTests.Handlers.TrainingRecommendationRequest.Commands;

public class DeleteTrainingRecommendationRequestCommandHandlerTests : UnitTestsBase<TrainingDbContext>
{
    private readonly DeleteTrainingRecommendationRequestCommandHandler _handler;

    public DeleteTrainingRecommendationRequestCommandHandlerTests()
        : base(TestDbContextFactory.CreateDbContext(nameof(DeleteTrainingRecommendationRequestCommandHandlerTests)))
    {
        _handler = new DeleteTrainingRecommendationRequestCommandHandler(dbContext);
    }

    [Fact]
    public async Task Handle_UnknownRequestId_ThrowsTrainingRecommendationRequestNotFoundException()
    {
        await Assert.ThrowsAsync<TrainingRecommendationRequestNotFoundException>(async () =>
        {
            await _handler.Handle(new DeleteTrainingRecommendationRequestCommand(), default);
        });
    }

    [Fact]
    public async Task? Handle_CompletedRequestWithoutRecommendations_RemovesRequestRecord()
    {
        var skills = new SkillFaker().Generate(2);
        await dbContext.AddRangeAsync(skills);

        var employees = new EmployeeFaker().Generate(2);
        await dbContext.AddRangeAsync(employees);

        var project = new ProjectFaker()
            .WithEmployees(employees)
            .WithRequiredSkills(skills)
            .Generate();
        await dbContext.AddAsync(project);

        var recommendationRequest = new TrainingRecommendationRequestFaker()
            .WithProject(project)
            .WithRequestStatus(RequestStatus.Completed)
            .Generate();

        await dbContext.AddAsync(recommendationRequest);
        await dbContext.SaveChangesAsync();

        var command = new DeleteTrainingRecommendationRequestCommand { Id = recommendationRequest.Id };
        await _handler.Handle(command, default);

        dbContext.RecommendationRequests.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_CompletedRequestWithRecommendations_RemovesRequestAndRecommendationResults()
    {
        var skill = new SkillFaker().Generate();
        await dbContext.AddAsync(skill);

        var employees = new EmployeeFaker().Generate(2);
        await dbContext.AddRangeAsync(employees);

        var project = new ProjectFaker()
            .WithEmployees(employees)
            .WithRequiredSkills(new List<Skill>() { skill })
            .Generate();
        await dbContext.AddAsync(project);

        var recommendationRequest = new TrainingRecommendationRequestFaker()
            .WithProject(project)
            .WithRequestStatus(RequestStatus.Completed)
            .Generate();
        await dbContext.AddAsync(recommendationRequest);

        var recommendationResults = new TrainingRecommendationResultFaker()
            .WithRequest(recommendationRequest)
            .WithSkill(skill)
            .Generate();
        await dbContext.AddRangeAsync(recommendationResults);
        await dbContext.SaveChangesAsync();

        var command = new DeleteTrainingRecommendationRequestCommand { Id = recommendationRequest.Id };
        await _handler.Handle(command, default);

        dbContext.RecommendationResults.Should().BeEmpty();
        dbContext.RecommendationRequests.Should().BeEmpty();
    }
}