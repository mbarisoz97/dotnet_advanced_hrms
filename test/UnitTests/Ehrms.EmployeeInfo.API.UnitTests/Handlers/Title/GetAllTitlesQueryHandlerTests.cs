using Ehrms.EmployeeInfo.API.Database.Context;
using Ehrms.EmployeeInfo.API.Handlers.Title.Query;
using Ehrms.EmployeeInfo.TestHelpers.Faker.Title.Model;

namespace Ehrms.EmployeeInfo.API.UnitTests.Handlers.Title;

public class GetAllTitlesQueryHandlerTests
{
    private readonly EmployeeInfoDbContext _dbContext;
    private readonly GetAllTitlesQueryHandler _handler;

    public GetAllTitlesQueryHandlerTests()
    {
        _dbContext = DbContextFactory.Create(Guid.NewGuid().ToString());
        _handler = new GetAllTitlesQueryHandler(_dbContext);
    }

    [Fact]
    public async Task Handle_ReturnsAllTitles()
    {
        var existingTitles = new TitleFaker().Generate(3);
        await _dbContext.AddRangeAsync(existingTitles);
        await _dbContext.SaveChangesAsync();

        var query = new GetAllTitlesQuery();
        var returnedTitles = await _handler.Handle(query, default);

        returnedTitles.Should().BeEquivalentTo(existingTitles);
    }
}