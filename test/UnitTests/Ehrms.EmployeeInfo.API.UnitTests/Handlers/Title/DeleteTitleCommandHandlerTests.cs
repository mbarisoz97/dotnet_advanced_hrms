using Ehrms.EmployeeInfo.API.Exceptions.Title;
using Ehrms.EmployeeInfo.API.Database.Context;
using Ehrms.EmployeeInfo.API.Handlers.Title.Command;
using Ehrms.EmployeeInfo.TestHelpers.Faker.Title.Model;
using Ehrms.EmployeeInfo.TestHelpers.Faker.Title.Command;

namespace Ehrms.EmployeeInfo.API.UnitTests.Handlers.Title;

public class DeleteTitleCommandHandlerTests
{
    private readonly EmployeeInfoDbContext _dbContext;
    private readonly DeleteTitleCommandHandler _handler;

    public DeleteTitleCommandHandlerTests()
    {
        _dbContext = DbContextFactory.Create(Guid.NewGuid().ToString());
        _handler = new(_dbContext);
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
}