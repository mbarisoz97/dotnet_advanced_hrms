using Ehrms.EmployeeInfo.API.Database.Context;
using Ehrms.EmployeeInfo.API.Exceptions.Title;
using Ehrms.EmployeeInfo.API.Handlers.Title.Command;
using Ehrms.EmployeeInfo.TestHelpers.Faker.Title.Model;
using Ehrms.EmployeeInfo.TestHelpers.Faker.Title.Command;

namespace Ehrms.EmployeeInfo.API.UnitTests.Handlers.Title;
public class CreateTitleCommandHandlerTests
{
    private readonly CreateTitleCommandHandler _handler;
    private readonly EmployeeInfoDbContext _dbContext;

    public CreateTitleCommandHandlerTests()
    {
        var mapper = MapperFactory.CreateWithExistingProfiles();
        _dbContext = DbContextFactory.Create(Guid.NewGuid().ToString());
        _handler = new(mapper, _dbContext);
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
}