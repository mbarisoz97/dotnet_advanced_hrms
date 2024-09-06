using Ehrms.Contracts.Title;
using Ehrms.Shared.TestHepers.Mock;
using Ehrms.EmployeeInfo.API.Database.Context;
using Ehrms.EmployeeInfo.API.Exceptions.Title;
using Ehrms.EmployeeInfo.API.Handlers.Title.Command;
using Ehrms.EmployeeInfo.TestHelpers.Faker.Title.Model;
using Ehrms.EmployeeInfo.TestHelpers.Faker.Title.Command;

namespace Ehrms.EmployeeInfo.API.UnitTests.Handlers.Title;
public class CreateTitleCommandHandlerTests
{
    private readonly EmployeeInfoDbContext _dbContext;
    private readonly CreateTitleCommandHandler _handler;
    private readonly Mock<IPublishEndpoint> _publishEndpointMock = new();

    public CreateTitleCommandHandlerTests()
    {
        var mapper = MapperFactory.CreateWithExistingProfiles();
        _dbContext = DbContextFactory.Create(Guid.NewGuid().ToString());
        _handler = new(mapper, _publishEndpointMock.Object, _dbContext);
    }

    [Fact]
    public async Task Handle_UniqueTitleName_ReturnsResultWithNewTitle()
    {
        var command = new CreatTitleCommandFaker().Generate();
        var result = await _handler.Handle(command, default);
        var titleInResult = result.Match<Database.Models.Title?>(s => s, static _ => null);

        titleInResult?.Id.Should().NotBe(Guid.Empty);
        titleInResult?.TitleName.Should().Be(command.TitleName);
    }

    [Fact]
    public async Task Handle_DuplicatedTitleName_ReturnsResultWithTitleNameInUseException()
    {
        var title = new TitleFaker().Generate();
        await _dbContext.AddAsync(title);
        await _dbContext.SaveChangesAsync();

        var command = new CreatTitleCommandFaker()
            .WithName(title.TitleName)
            .Generate();
        var result = await _handler.Handle(command, default);
        var exceptionInResult = result.Match<Exception?>(_ => null, f => f);

        exceptionInResult.Should().BeOfType<TitleNameInUseException>();
    }

    [Fact]
    public async Task Handle_SuccessfullyCreatedTitle_PublishesTitleCreatedEvent()
    {
        _publishEndpointMock.Setup(x => x.Publish(It.IsAny<TitleCreatedEvent>(), It.IsAny<CancellationToken>()));
        var command = new CreatTitleCommandFaker().Generate();

        await _handler.Handle(command, default);

        _publishEndpointMock.Verify(x =>
            x.Publish(It.IsAny<TitleCreatedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_CreatTitleFailed_PublishesNoEvent()
    {
        var title = new TitleFaker().Generate();
        await _dbContext.AddAsync(title);
        await _dbContext.SaveChangesAsync();

        var command = new CreatTitleCommandFaker()
            .WithName(title.TitleName)
            .Generate();

        _publishEndpointMock.Setup(x => x.Publish(It.IsAny<TitleCreatedEvent>(), It.IsAny<CancellationToken>()));
        await _handler.Handle(command, default);

        _publishEndpointMock.Verify(x =>
            x.Publish(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}