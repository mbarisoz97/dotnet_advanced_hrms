using Ehrms.Contracts.Title;
using Ehrms.EmployeeInfo.API.Database.Context;
using Ehrms.EmployeeInfo.API.Exceptions.Title;

namespace Ehrms.EmployeeInfo.API.UnitTests.Handlers.Title;

public class UpdateTitleCommandHandlerTests
{
    private readonly EmployeeInfoDbContext _dbContext;
    private readonly UpdateTitleCommandHandler _handler;
    private readonly Mock<IPublishEndpoint> _publishEndpointMock = new();

    public UpdateTitleCommandHandlerTests()
    {
        var _mapper = MapperFactory.CreateWithExistingProfiles();
        _dbContext = DbContextFactory.Create(Guid.NewGuid().ToString());
        _handler = new(_mapper, _publishEndpointMock.Object, _dbContext);
    }

    [Fact]
    public async Task Handle_NonExistingTitleId_ReturnsResultWithTitleNotFoundException()
    {
        var command = new UpdateTitleCommandFaker().Generate();
        var commandResult = await _handler.Handle(command, default);
        var exceptionInResult = commandResult.Match<Exception?>(_ => null, f => f);

        exceptionInResult.Should().BeOfType<TitleNotFoundException>();
    }

    [Fact]
    public async Task Handle_ExistingTitleId_ReturnsResultWithUpdatedTitle()
    {
        var existingTitle = new TitleFaker().Generate();
        await _dbContext.AddAsync(existingTitle);
        await _dbContext.SaveChangesAsync();

        var command = new UpdateTitleCommandFaker()
            .WithId(existingTitle.Id)
            .Generate();
        var commandResult = await _handler.Handle(command, default);
        var titleInResult = commandResult.Match<Database.Models.Title?>(s => s, _ => null);

        titleInResult.Should().BeEquivalentTo(command);
    }

    [Fact]
    public async Task Handle_SuccessfullyUpdatedTitle_PublishesTitleUpdatedEvent()
    {
        var existingTitle = new TitleFaker().Generate();
        await _dbContext.AddAsync(existingTitle);
        await _dbContext.SaveChangesAsync();

        _publishEndpointMock.Setup(x =>
            x.Publish(It.IsAny<TitleUpdatedEvent>(), It.IsAny<CancellationToken>()));

        var command = new UpdateTitleCommandFaker()
            .WithId(existingTitle.Id)
            .Generate();
        await _handler.Handle(command, default);

        _publishEndpointMock.Verify(x => 
            x.Publish(It.IsAny<TitleUpdatedEvent>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_FailedTitleUpdate_PublishesNoEvent()
    {
        _publishEndpointMock.Setup(x =>
            x.Publish(It.IsAny<TitleUpdatedEvent>(), It.IsAny<CancellationToken>()));

        var command = new UpdateTitleCommandFaker().Generate();
        await _handler.Handle(command, default);

        _publishEndpointMock.Verify(x =>
            x.Publish(It.IsAny<object>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}