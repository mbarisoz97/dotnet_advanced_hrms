using Moq;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Ehrms.TrainingManagement.API.UnitTests.Consumers.Training;

public class TrainingRecommendationRequestAcceptedEventConsumerTests : UnitTestsBase<TrainingDbContext>
{
    private readonly TrainingRecommendationRequestAcceptedEventConsumer _consumer;
    private readonly Mock<ILogger<TrainingRecommendationRequestAcceptedEventConsumer>> _loggerMock = new();

    public TrainingRecommendationRequestAcceptedEventConsumerTests()
        : base(TestDbContextFactory.CreateDbContext(nameof(TrainingRecommendationRequestCompletedEventConsumerTests)))
    {
        _consumer = new(dbContext, _loggerMock.Object);
    }

    [Fact]
    public async Task Consume_SuccessfullyCreatedTrainingRecommendation_PublishesTrainingRecommendationCompletedEvent()
    {
        var project = new ProjectFaker().Generate();
        var request = new TrainingRecommendationRequestFaker()
            .WithProject(project)
            .Generate();
        await dbContext.AddRangeAsync(project, request);
        await dbContext.SaveChangesAsync();

        var requestAcceptedEvent = new TrainingRecommendationAcceptedEventFaker()
            .WithRequestId(request.Id)
            .Generate();

        Mock<ConsumeContext<TrainingRecommendationRequestAcceptedEvent>> contextMock = new();
        contextMock.Setup(x => x.Message)
            .Returns(requestAcceptedEvent);

        await _consumer.Consume(contextMock.Object);
        contextMock.Verify(
            x => x.Publish(It.IsAny<TrainingRecommendationCompletedEvent>(), It.IsAny<CancellationToken>()),
            Times.Once);
        contextMock.Verify(
            x => x.Publish(It.IsAny<TrainingRecommendationCancelledEvent>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Consume_UnknownProjectId_PublishesTrainingRecommendationCancelledEvent()
    {
        var request = new TrainingRecommendationRequestFaker().Generate();
        await dbContext.AddRangeAsync(request);
        await dbContext.SaveChangesAsync();

        var requestAcceptedEvent = new TrainingRecommendationAcceptedEventFaker()
            .WithRequestId(request.Id)
            .Generate();

        Mock<ConsumeContext<TrainingRecommendationRequestAcceptedEvent>> contextMock = new();
        contextMock.Setup(x => x.Message)
            .Returns(requestAcceptedEvent);

        await _consumer.Consume(contextMock.Object);
        contextMock.Verify(
            x => x.Publish(It.IsAny<TrainingRecommendationCancelledEvent>(), It.IsAny<CancellationToken>()),
            Times.Once);
        contextMock.Verify(
            x => x.Publish(It.IsAny<TrainingRecommendationCompletedEvent>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Consume_ValidRecommendationRequest_SetsRequestStatusAsPending()
    {
        var project = new ProjectFaker().Generate();
        var request = new TrainingRecommendationRequestFaker()
            .WithRequestStatus(RequestStatus.Accepted)
            .WithProject(project)
            .Generate();
        await dbContext.AddRangeAsync(project, request);
        await dbContext.SaveChangesAsync();

        var requestAcceptedEvent = new TrainingRecommendationAcceptedEventFaker()
            .WithRequestId(request.Id)
            .Generate();

        Mock<ConsumeContext<TrainingRecommendationRequestAcceptedEvent>> contextMock = new();
        contextMock.Setup(x => x.Message)
            .Returns(requestAcceptedEvent);
        await _consumer.Consume(contextMock.Object);

        request.RequestStatus.Should().Be(RequestStatus.Pending);
    }

    [Fact]
    public async Task Consume_ProjectWithNoTrainingRecommendationPreferences_DoesNotCreateRecommendations()
    {
        var employees = new EmployeeFaker().Generate(2);
        await dbContext.Employees.AddRangeAsync(employees);

        //Create project and required project skills
        var requiredProjectSkills = new SkillFaker().Generate(2);
        await dbContext.Skills.AddRangeAsync(requiredProjectSkills);
        var project = new ProjectFaker()
            .WithEmployees(employees)
            .WithRequiredSkills(requiredProjectSkills)
            .Generate();
        await dbContext.Projects.AddAsync(project);

        //Create training recommendation request 
        var request = new TrainingRecommendationRequestFaker()
            .WithProject(project)
            .Generate();
        await dbContext.AddRangeAsync(request);
        await dbContext.SaveChangesAsync();

        var requestAcceptedEvent = new TrainingRecommendationAcceptedEventFaker()
            .WithRequestId(request.Id)
            .Generate();
        Mock<ConsumeContext<TrainingRecommendationRequestAcceptedEvent>> contextMock = new();
        contextMock.Setup(x => x.Message)
            .Returns(requestAcceptedEvent);
        await _consumer.Consume(contextMock.Object);

        dbContext.RecommendationResults.Count().Should().Be(0);
    }

    [Fact]
    public async Task Consume_EmployeesWithoutRequiredSkills_And_ProjectWithTrainingRecommendationPreferences_CreatesRecommendationResults()
    {
        var title = new TitleFaker().Generate();
        await dbContext.AddAsync(title);

        var employees = new EmployeeFaker()
            .WithTitle(title)
            .Generate(2);
        await dbContext.Employees.AddRangeAsync(employees);

        //Create project and required project skills
        var requiredProjectSkills = new SkillFaker().Generate(2);
        await dbContext.Skills.AddRangeAsync(requiredProjectSkills);
        var project = new ProjectFaker()
            .WithEmployees(employees)
            .WithRequiredSkills(requiredProjectSkills)
            .Generate();
        await dbContext.Projects.AddAsync(project);

        //Create training recommendation request 
        var request = new TrainingRecommendationRequestFaker()
            .WithProject(project)
            .Generate();
        await dbContext.AddRangeAsync(request);

        var trainingPreference = new TrainingRecommendationPreferencesFaker()
            .WithProject(project)
            .WithTitle(title)
            .WithSkills(requiredProjectSkills)
            .Generate();
        await dbContext.AddAsync(trainingPreference);

        await dbContext.SaveChangesAsync();

        var requestAcceptedEvent = new TrainingRecommendationAcceptedEventFaker()
            .WithRequestId(request.Id)
            .Generate();
        Mock<ConsumeContext<TrainingRecommendationRequestAcceptedEvent>> contextMock = new();
        contextMock.Setup(x => x.Message)
            .Returns(requestAcceptedEvent);
        
        await _consumer.Consume(contextMock.Object);

        dbContext.RecommendationResults.Count().Should().Be(2);
    }

    [Fact]
    public async Task Consume_EmployeesWithRequiredSkills_And_ProjectWithTrainingRecommendationPreferences_CreatesRecommendationResults()
    {
        var title = new TitleFaker().Generate();
        await dbContext.AddAsync(title);

        var skills = new SkillFaker().Generate(2);
        await dbContext.AddRangeAsync(skills);

        var employees = new EmployeeFaker()
            .WithTitle(title)
            .WithSkills(skills)
            .Generate(2);
        await dbContext.Employees.AddRangeAsync(employees);

        var project = new ProjectFaker()
            .WithEmployees(employees)
            .WithRequiredSkills(skills)
            .Generate();
        await dbContext.Projects.AddAsync(project);

        var request = new TrainingRecommendationRequestFaker()
            .WithProject(project)
            .Generate();
        await dbContext.AddRangeAsync(request);

        var trainingPreference = new TrainingRecommendationPreferencesFaker()
            .WithProject(project)
            .WithTitle(title)
            .WithSkills(skills)
            .Generate();
        await dbContext.AddAsync(trainingPreference);

        await dbContext.SaveChangesAsync();

        var requestAcceptedEvent = new TrainingRecommendationAcceptedEventFaker()
            .WithRequestId(request.Id)
            .Generate();
        Mock<ConsumeContext<TrainingRecommendationRequestAcceptedEvent>> contextMock = new();
        contextMock.Setup(x => x.Message)
            .Returns(requestAcceptedEvent);

        await _consumer.Consume(contextMock.Object);

        dbContext.RecommendationResults.Count().Should().Be(0);
    }
}