using Moq;
using MassTransit;
using Ehrms.Contracts.Title;
using Microsoft.Extensions.Logging;
using Ehrms.TrainingManagement.API.MessageQueue.Consumers.TitleEvent;

namespace Ehrms.TrainingManagement.API.UnitTests.Consumers.Title;

public class TitleCreateEventConsumerTests
{
    private readonly TrainingDbContext _dbContext;
    private readonly TitleCreatedEventConsumer _consumer;
    private readonly Mock<ILogger<TitleCreatedEventConsumer>> _loggerMock = new();

    public TitleCreateEventConsumerTests()
    {
        var mapper = MapperFactory.CreateWithExistingProfiles();
        _dbContext = TestDbContextFactory.CreateDbContext(Guid.NewGuid().ToString());
        _consumer = new(mapper, _dbContext, _loggerMock.Object);
    }

    [Fact]
    public async Task Consume_NonExistingTitle_AddsNewTitle()
    {
        var titleCreatedEvent = new TitleCreatedEventFaker().Generate();
        Mock<ConsumeContext<TitleCreatedEvent>> contextMock = new();
        contextMock.Setup(x => x.Message).Returns(titleCreatedEvent);

        await _consumer.Consume(contextMock.Object);

        _dbContext.Titles.Count().Should().Be(1);
    }

    [Fact]
    public async Task Consume_ExistingTitle_IgnoresEvent()
    {
        var title = new TitleFaker().Generate();
        await _dbContext.AddAsync(title);
        await _dbContext.SaveChangesAsync();

        var titleCreatedEvent = new TitleCreatedEventFaker()
            .WithName(title.Name)
            .Generate();
        Mock<ConsumeContext<TitleCreatedEvent>> contextMock = new();
        contextMock.Setup(x => x.Message).Returns(titleCreatedEvent);

        await _consumer.Consume(contextMock.Object);

        _dbContext.Titles.Count().Should().Be(1);
    }
}