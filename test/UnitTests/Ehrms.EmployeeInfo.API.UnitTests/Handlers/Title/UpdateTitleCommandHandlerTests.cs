using Ehrms.EmployeeInfo.API.Database.Context;
using Ehrms.EmployeeInfo.API.Exceptions.Title;
using Ehrms.EmployeeInfo.API.Handlers.Title.Command;
using Ehrms.EmployeeInfo.TestHelpers.Faker.Title.Model;
using Ehrms.EmployeeInfo.TestHelpers.Faker.Title.Command;

namespace Ehrms.EmployeeInfo.API.UnitTests.Handlers.Title;

public class UpdateTitleCommandHandlerTests
{
    private readonly EmployeeInfoDbContext _dbContext;
    private readonly UpdateTitleCommandHandler _handler;

    public UpdateTitleCommandHandlerTests()
    {
        var _mapper = MapperFactory.CreateWithExistingProfiles();
        _dbContext = DbContextFactory.Create(Guid.NewGuid().ToString());
        _handler = new(_mapper, _dbContext);
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
}