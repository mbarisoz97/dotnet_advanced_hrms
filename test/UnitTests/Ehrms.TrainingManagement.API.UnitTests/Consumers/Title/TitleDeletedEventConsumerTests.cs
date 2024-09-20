using Ehrms.Contracts.Title;
using Ehrms.TrainingManagement.API.MessageQueue.Consumers.TitleEvent;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;

namespace Ehrms.TrainingManagement.API.UnitTests.Consumers.Title;

public class TitleDeletedEventConsumerTests
{
    private readonly TrainingDbContext _dbContext;
    private readonly TitleDeletedEventConsumer _consumer;
    private readonly Mock<ILogger<TitleDeletedEventConsumer>> _loggerMock = new();

    public TitleDeletedEventConsumerTests()
    {
        _dbContext = TestDbContextFactory.CreateDbContext(Guid.NewGuid().ToString());
        _consumer = new TitleDeletedEventConsumer(_dbContext, _loggerMock.Object);
    }

    [Fact]
    public async Task Consume_NonExistingTitleId_IgnoresEvent()
    {
        var titleDeletedEvent = new TitleDeletedEventFaker().Generate();

        Mock<ConsumeContext<TitleDeletedEvent>> contextMock = new();
        contextMock.Setup(x => x.Message).Returns(titleDeletedEvent);

        await _consumer.Consume(contextMock.Object);

        _dbContext.Titles.Count().Should().Be(0);   
    }

    [Fact]
    public async Task Consume_ExistingTitleId_RemovesTitlet()
    {
        var title = new TitleFaker().Generate();
        await _dbContext.AddAsync(title);
        await _dbContext.SaveChangesAsync();

        var titleDeletedEvent = new TitleDeletedEventFaker()
            .WithId(title.Id)
            .Generate();

        Mock<ConsumeContext<TitleDeletedEvent>> contextMock = new();
        contextMock.Setup(x => x.Message).Returns(titleDeletedEvent);

        await _consumer.Consume(contextMock.Object);

        _dbContext.Titles.Count().Should().Be(0);
    }
}