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
    public async Task Consume_EmployeesWithoutRequiredSkills_CreatesNewTrainingRecommendationsForEmployees()
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

        dbContext.RecommendationResults.Count().Should().Be(requiredProjectSkills.Count);
    }
    
    [Fact]
    public async Task Consume_EmployeesWithRequiredSkills_ShouldCreateNoTrainingRecommendation()
    {
        var requiredProjectSkills = new SkillFaker().Generate(2);
        await dbContext.Skills.AddRangeAsync(requiredProjectSkills);

        var employees = new EmployeeFaker()
            .WithSkills(requiredProjectSkills)
            .Generate(2);
        await dbContext.Employees.AddRangeAsync(employees);

        //Create project and required project skills
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

        dbContext.RecommendationResults.Should().BeEmpty();
    }
}