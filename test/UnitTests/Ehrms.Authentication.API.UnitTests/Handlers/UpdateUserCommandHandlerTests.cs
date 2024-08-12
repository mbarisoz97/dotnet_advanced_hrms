using Ehrms.Authentication.API.Adapter;
using Ehrms.Authentication.API.Database.Context;
using Ehrms.Authentication.TestHelpers.Faker.Models;
using Ehrms.Authentication.API.UnitTests.TestHelpers;

namespace Ehrms.Authentication.API.UnitTests.Handlers;

public class UpdateUserCommandHandlerTests
{
    private readonly UpdateUserCommandHandler _handler;
    private readonly ApplicationUserDbContext _userDbContext;
    private readonly Mock<IUserManagerAdapter> _userManagerMock = new();

    public UpdateUserCommandHandlerTests()
    {
        _userDbContext = TestDbContextFactory.CreateDbContext(nameof(UpdateUserCommandHandlerTests));
        var _mapper = MapperFactory.CreateWithExistingProfiles();
        _handler = new(_userManagerMock.Object, _mapper, _userDbContext);
    }

    [Fact]
    public async Task Handle_NonExistingUserId_ReturnsWithUserNotFoundException()
    {
        var command = new UpdateUserCommandFaker().Generate();
        var result = await _handler.Handle(command, default);

        result.IsFaulted.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_FailedUserUpdate_ReturnsWithUserUpdateFailedException()
    {
        User user = await AddRandomUser();
        var command = new UpdateUserCommandFaker()
            .WithId(user.Id)
            .WithAccountStatus(true)
            .Generate();

        _userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<User>()))
            .ReturnsAsync(IdentityResult.Failed());

        var result = await _handler.Handle(command, default);
        result.IsFaulted.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_UserUpdatedSucceded_ReturnsUserWithNoExceptions()
    {
        User user = await AddRandomUser();
        var command = new UpdateUserCommandFaker()
            .WithId(user.Id)
            .WithAccountStatus(true)
            .Generate();

        _userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<User>()))
            .ReturnsAsync(IdentityResult.Success);

        var handleResult = await _handler.Handle(command, default);

        handleResult.IsSuccess.Should().BeTrue();
        var readUserDto = handleResult.Match(s => s, f => null);

        readUserDto.Should().BeEquivalentTo(command);
    }

    private async Task<User> AddRandomUser()
    {
        var user = new UserFaker()
            .WithAccountStatus(false)
            .Generate();

        await _userDbContext.AddAsync(user);
        await _userDbContext.SaveChangesAsync();
        return user;
    }
}