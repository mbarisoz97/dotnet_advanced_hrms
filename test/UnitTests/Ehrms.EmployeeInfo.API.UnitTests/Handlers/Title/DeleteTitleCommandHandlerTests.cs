namespace Ehrms.EmployeeInfo.API.UnitTests.Handlers.Title;

public class DeleteTitleCommandHandlerTests
{
    private readonly EmployeeInfoDbContext _dbContext;
    private readonly DeleteTitleCommandHandler _handler;
    private readonly Mock<IPublishEndpoint> _publishEndpointMock = new();

    public DeleteTitleCommandHandlerTests()
    {
        var mapper = MapperFactory.CreateWithExistingProfiles();
        _dbContext = DbContextFactory.Create(Guid.NewGuid().ToString());
        _handler = new(mapper, _publishEndpointMock.Object, _dbContext);
    }

    [Fact]
    public async Task Handle_ExistingTitleId_DeletesTitle()
    {
        var title = new TitleFaker().Generate();
        await _dbContext.AddAsync(title);
        await _dbContext.SaveChangesAsync();

        var deleteTitleCommand = new DeleteTitleCommandFaker()
            .WithId(title.Id)
            .Generate();

        await _handler.Handle(deleteTitleCommand, default);
        _dbContext.Titles.Count().Should().Be(0);
    }

    [Fact]
    public async Task Handle_NonExistingTitleId_ReturnsResultWithTitleNotFoundException()
    {
        var command = new DeleteTitleCommandFaker().Generate();
        var commandResult = await _handler.Handle(command, default);
        var exceptionInResult = commandResult.Match<Exception?>(_ => null, f => f);

        exceptionInResult.Should().BeOfType<TitleNotFoundException>();
    }


    [Fact]
    public async Task Handle_SuccessfullyDeletedTitle_PublishesTitleDeletedEvent()
    {
        var title = new TitleFaker().Generate();
        await _dbContext.AddAsync(title);
        await _dbContext.SaveChangesAsync();

        var deleteTitleCommand = new DeleteTitleCommandFaker()
            .WithId(title.Id)
            .Generate();

        _publishEndpointMock.Setup(x => x.Publish(
            It.IsAny<TitleDeletedEvent>(), It.IsAny<CancellationToken>()));

        await _handler.Handle(deleteTitleCommand, default);

        _publishEndpointMock.Verify(x => x.Publish(
            It.IsAny<TitleDeletedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DeleteTitleFails_PublishesNoEvent()
    {
        var deleteTitleCommand = new DeleteTitleCommandFaker().Generate();

        _publishEndpointMock.Setup(x => x.Publish(
            It.IsAny<TitleDeletedEvent>(), It.IsAny<CancellationToken>()));

        await _handler.Handle(deleteTitleCommand, default);

        _publishEndpointMock.Verify(x => x.Publish(
            It.IsAny<TitleDeletedEvent>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}