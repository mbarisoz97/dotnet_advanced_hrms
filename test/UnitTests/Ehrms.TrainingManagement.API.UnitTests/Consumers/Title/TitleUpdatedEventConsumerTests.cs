using Moq;
using Ehrms.Contracts.Title;
using Microsoft.Extensions.Logging;
using Ehrms.TrainingManagement.API.MessageQueue.Consumers.TitleEvent;
using MassTransit;

namespace Ehrms.TrainingManagement.API.UnitTests.Consumers.Title;

public class TitleUpdatedEventConsumerTests
{
    private readonly TrainingDbContext _dbContext;
    private readonly TitleUpdatedEventConsumer _consumer;
    private readonly Mock<ILogger<TitleUpdatedEventConsumer>> _loggerMock = new();

    public TitleUpdatedEventConsumerTests()
    {
        var mapper = MapperFactory.CreateWithExistingProfiles();
        _dbContext = TestDbContextFactory.CreateDbContext(Guid.NewGuid().ToString());
        _consumer = new TitleUpdatedEventConsumer(mapper, _dbContext, _loggerMock.Object);
    }

    [Fact]
    public async Task Consume_NonExistingTitleId_IgnoresEvent()
    {
        var titleCreatedEvent = new TitleUpdatedEventFaker().Generate();
        Mock<ConsumeContext<TitleUpdatedEvent>> contextMock = new();
        contextMock.Setup(x => x.Message).Returns(titleCreatedEvent);

        await _consumer.Consume(contextMock.Object);
        _dbContext.Titles.Count().Should().Be(0);
    }

    [Fact]
    public async Task Consume_ExistingTitleId_UpdatesTitle()
    {
        var title = new TitleFaker().Generate();
        await _dbContext.AddAsync(title);
        await _dbContext.SaveChangesAsync();

        var titleUpdatedEvent = new TitleUpdatedEventFaker()
            .WithId(title.Id)
            .WithName("UpdateTest")
            .Generate();

        Mock<ConsumeContext<TitleUpdatedEvent>> contextMock = new();
        contextMock.Setup(x => x.Message).Returns(titleUpdatedEvent);

        await _consumer.Consume(contextMock.Object);
        title.Should().BeEquivalentTo(titleUpdatedEvent);
    }
}