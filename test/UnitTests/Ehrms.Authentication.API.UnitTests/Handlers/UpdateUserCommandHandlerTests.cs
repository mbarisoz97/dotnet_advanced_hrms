using System.Security.Claims;
using Ehrms.Authentication.API.Database.Context;
using Ehrms.Authentication.API.Exceptions;
using Ehrms.Authentication.TestHelpers.Faker.Models;
using Ehrms.Authentication.API.UnitTests.TestHelpers;
using Ehrms.Authentication.API.UnitTests.TestHelpers.Mock;

namespace Ehrms.Authentication.API.UnitTests.Handlers;

public class UpdateUserCommandHandlerTests
{
    private readonly UpdateUserCommandHandler _handler;
    private readonly ApplicationUserDbContext _userDbContext;

    private readonly MockUserManager _mockUserManager;
    private readonly MockHttpContextAccessor _mockHttpContextAccessor;

    public UpdateUserCommandHandlerTests()
    {
        var _mapper = MapperFactory.CreateWithExistingProfiles();
        _userDbContext = TestDbContextFactory.CreateDbContext(nameof(UpdateUserCommandHandlerTests));

        _mockUserManager = new(_userDbContext);
        _mockHttpContextAccessor = new();

        _handler = new(_mockUserManager.Object, _mockHttpContextAccessor.Object, _mapper, _userDbContext);
    }

    [Fact]
    public async Task Handle_NonExistingUserId_ReturnsWithUserNotFoundException()
    {
        var command = new UpdateUserCommandFaker().Generate();
        var result = await _handler.Handle(command, default);

        result.IsFaulted.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_UserTryingToUpdateItsOwnAccount_ReturnsResultWithUserUpdateNotAllowedException()
    {
        var user = new UserFaker().Generate();
        await _userDbContext.AddAsync(user);
        await _userDbContext.SaveChangesAsync();
        
        _mockHttpContextAccessor.SetupHttpContextForUser(user.UserName!);
        
        var command = new UpdateUserCommandFaker()
            .WithId(user.Id)
            .Generate();
        
        var result = await _handler.Handle(command, default);
        var exceptionInResult = result.Match<Exception?>(s => null, f => f);
        
        result.IsFaulted.Should().BeTrue();
        exceptionInResult.Should().BeOfType<UserUpdateNotAllowedException>();
    }
    
    [Fact]
    public async Task Handle_UnidentifiedRequestOwner_ReturnsResultWithUserUpdateNotAllowedException()
    {
        _mockHttpContextAccessor.SetupHttpContextForUser(string.Empty);
        
        var command = new UpdateUserCommandFaker().Generate();
        
        var result = await _handler.Handle(command, default);
        var exceptionInResult = result.Match<Exception?>(s => null, f => f);
        
        result.IsFaulted.Should().BeTrue();
        exceptionInResult.Should().BeOfType<UserUpdateNotAllowedException>();
    }

    [Fact]
    public async Task Handle_FailedUserUpdate_ReturnsWithUserUpdateFailedException()
    {
        var user = new UserFaker()
            .WithAccountStatus(false)
            .Generate();
        await _userDbContext.AddAsync(user);
        await _userDbContext.SaveChangesAsync();

        _mockUserManager.SetupUpdateAsync(IdentityResult.Failed());

        var command = new UpdateUserCommandFaker()
            .WithId(user.Id)
            .WithAccountStatus(true)
            .Generate();

        var result = await _handler.Handle(command, default);
        result.IsFaulted.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_UserUpdatedSucceded_ReturnsUserWithNoExceptions()
    {
        var roles = new RoleFaker().Generate(3);
        await _userDbContext.AddRangeAsync(roles);

        var user = new UserFaker()
            .WithAccountStatus(false)
            .Generate();
        await _userDbContext.AddAsync(user);
        await _userDbContext.SaveChangesAsync();

        _mockUserManager.SetupUpdateAsync(IdentityResult.Success);

        var command = new UpdateUserCommandFaker()
            .WithId(user.Id)
            .WithRoles(roles)
            .WithAccountStatus(true)
            .Generate();

        var handleResult = await _handler.Handle(command, default);
        handleResult.IsSuccess.Should().BeTrue();

        var updatedUser = handleResult.Match<User?>(s => s, f => null);

        updatedUser.Should().BeEquivalentTo(command, opt => opt.Excluding(u => u.Roles));
        updatedUser?.UserRoles.Select(r => r.Role!.Name)
            .Should().BeEquivalentTo(command.Roles);
    }
}