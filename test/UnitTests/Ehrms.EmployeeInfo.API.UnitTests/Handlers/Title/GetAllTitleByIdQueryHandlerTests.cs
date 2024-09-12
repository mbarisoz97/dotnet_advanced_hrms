using Ehrms.EmployeeInfo.API.Database.Context;
using Ehrms.EmployeeInfo.API.Exceptions.Title;

namespace Ehrms.EmployeeInfo.API.UnitTests.Handlers.Title;

public class GetAllTitleByIdQueryHandlerTests
{
    private readonly EmployeeInfoDbContext _dbContext;
    private readonly GetTitleByIdQueryHandler _handler;

    public GetAllTitleByIdQueryHandlerTests()
    {
        _dbContext = DbContextFactory.Create();
        _handler = new GetTitleByIdQueryHandler(_dbContext);
    }

    [Fact]
    public async Task Handle_NonExistingTitleId_ReturnsResultWithTitleNotFoundException()
    {
        var query = new GetTitleByIdQuery(Guid.NewGuid());
        var queryResult = await _handler.Handle(query, default);
        var expectionInResult = queryResult.Match<Exception?>(_ => null, f => f);

        expectionInResult.Should().BeOfType<TitleNotFoundException>();
    }

    [Fact]
    public async Task Handle_ExistingTitleId_ReturnsResultWithTitle()
    {
        var title = new TitleFaker().Generate();
        await _dbContext.AddRangeAsync(title);
        await _dbContext.SaveChangesAsync();

        var query = new GetTitleByIdQuery(title.Id);
        var queryResult = await _handler.Handle(query, default);
        var titleInResult = queryResult.Match<Database.Models.Title?>(s => s, _ => null);

        titleInResult.Should().Be(title);
    }
}