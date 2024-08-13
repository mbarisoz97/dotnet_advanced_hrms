using Ehrms.Authentication.API.Database.Context;
using Ehrms.Authentication.TestHelpers.Faker.Models;
using Ehrms.Authentication.API.Handlers.User.Queries;
using Ehrms.Authentication.API.UnitTests.TestHelpers;

namespace Ehrms.Authentication.API.UnitTests.Handlers;

public class GetUserByIdQueryHandlerTests
{
    private readonly GetUserByIdQueryHandler _handler;
    private readonly ApplicationUserDbContext _userDbContext;

    public GetUserByIdQueryHandlerTests()
    {
        _userDbContext = TestDbContextFactory.CreateDbContext(nameof(GetUserByIdQueryHandler));
        _handler = new(_userDbContext);
    }

    [Fact]
    public async Task Handle_NonExistingUserId_ReturnsResultWithUserNotFoundException()
    {
        var query = new GetUserByIdQuery() { Id = Guid.NewGuid() };
        var result = await _handler.Handle(query, default);

        result.IsFaulted.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ExistingUserId_ReturnsResultWithUser()
    {
        var user = new UserFaker().Generate();
        await _userDbContext.AddAsync(user);
        await _userDbContext.SaveChangesAsync();

        var query = new GetUserByIdQuery() { Id = user.Id};

        var result = await _handler.Handle(query, default);
        result.IsSuccess.Should().BeTrue();

        var returnedUser = result.Match<User?>(s => s, f => null);
        returnedUser.Should().BeEquivalentTo(user);
    }
}