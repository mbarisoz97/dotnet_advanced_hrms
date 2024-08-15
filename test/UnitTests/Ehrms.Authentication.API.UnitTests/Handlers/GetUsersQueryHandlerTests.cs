using Ehrms.Authentication.API.Database.Context;
using Ehrms.Authentication.TestHelpers.Faker.Models;
using Ehrms.Authentication.API.Handlers.User.Queries;
using Ehrms.Authentication.API.UnitTests.TestHelpers;

namespace Ehrms.Authentication.API.UnitTests.Handlers;

public class GetUsersQueryHandlerTests
{
    private readonly GetUsersQueryHandler _handler;
    private readonly ApplicationUserDbContext _userDbContext;

    public GetUsersQueryHandlerTests()
    {
        _userDbContext = TestDbContextFactory.CreateDbContext(nameof(GetUserByIdQueryHandlerTests));
        _handler = new(_userDbContext);
    }

    [Fact]
    public async Task Handle_NonEmptyUserList_ReturnsAllExistingUsers()
    {
        var users = new UserFaker().Generate(10);
        await _userDbContext.AddRangeAsync(users);
        await _userDbContext.SaveChangesAsync();

        var returnedUsers = await _handler.Handle(new GetUsersQuery(), default);
        returnedUsers.Should().BeEquivalentTo(users);
    }
}