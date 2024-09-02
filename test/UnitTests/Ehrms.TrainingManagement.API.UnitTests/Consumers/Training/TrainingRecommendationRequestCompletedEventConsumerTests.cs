using Moq;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Ehrms.TrainingManagement.API.UnitTests.Consumers.Training;

public class TrainingRecommendationRequestCompletedEventConsumerTests : UnitTestsBase<TrainingDbContext>
{
    private readonly TrainingRecommendationCompletedEventConsumer _eventConsumer;
    private readonly Mock<ILogger<TrainingRecommendationCompletedEventConsumer>> _loggerMock = new();

    public TrainingRecommendationRequestCompletedEventConsumerTests()
        : base(TestDbContextFactory.CreateDbContext(Guid.NewGuid().ToString()))
    {
        _eventConsumer = new(dbContext, _loggerMock.Object);
    }

    [Fact]
    public async Task Consume_ValidRequestId_SetsRequestStatusAsCompleted()
    {
        var recommendationRequest = new TrainingRecommendationRequestFaker().Generate();
        await dbContext.RecommendationRequests.AddAsync(recommendationRequest);
        await dbContext.SaveChangesAsync();

        var recommendationCompletedEvent = new TrainingRecommendationCompletedEventFaker()
            .WithRequestId(recommendationRequest.Id)
            .Generate();

        Mock<ConsumeContext<TrainingRecommendationCompletedEvent>> contextMock = new();
        contextMock.Setup(x => x.Message)
            .Returns(recommendationCompletedEvent);
        await _eventConsumer.Consume(contextMock.Object);

        recommendationRequest.RequestStatus.Should().Be(RequestStatus.Completed);
    }

    [Fact]
    public async Task Consume_InvalidRequestId_IgnoresRecommendationCompletedEvent()
    {
        const RequestStatus initialRequestStatus = RequestStatus.Accepted;
        var recommendationRequest = new TrainingRecommendationRequestFaker()
            .WithRequestStatus(initialRequestStatus)
            .Generate();

        await dbContext.RecommendationRequests.AddAsync(recommendationRequest);
        await dbContext.SaveChangesAsync();

        recommendationRequest.RequestStatus.Should().Be(initialRequestStatus);
    }
}