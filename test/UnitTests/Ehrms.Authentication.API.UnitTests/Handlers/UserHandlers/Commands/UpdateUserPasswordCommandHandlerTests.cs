namespace Ehrms.Authentication.API.UnitTests.Handlers.UserHandlers.Commands;

public class UpdateUserPasswordCommandHandlerTests
{
    private readonly MockUserManager _mockUserManager;
    private readonly UpdateUserPasswordCommandHandler _handler;

    public UpdateUserPasswordCommandHandlerTests()
    {
        var dbContext = TestDbContextFactory.CreateDbContext(nameof(UpdateUserPasswordCommandHandlerTests));
        _mockUserManager = new MockUserManager(dbContext);
        _handler = new UpdateUserPasswordCommandHandler(_mockUserManager.Object);
    }

    [Fact]
    public async Task Handle_UnknownUsername_ReturnsResultWithUserNotFoundException()
    {
        _mockUserManager.SetupFindByNameAsync();
        var command = new UpdateUserPasswordCommandFaker().Generate();
        var commandResult = await _handler.Handle(command, default);
        var exceptionInResult = commandResult.Match<Exception?>(_ => null, f => f);

        commandResult.IsFaulted.Should().BeTrue();
        exceptionInResult.Should().BeOfType<UserNotFoundException>();
    }

    [Fact]
    public async Task Handle_Inactive_ReturnsResultWithUserNotFoundException()
    {
        var user = new UserFaker()
            .WithAccountStatus(false)
            .Generate();
        _mockUserManager.SetupFindByNameAsync(user);
        _mockUserManager.SetupCheckPasswordAsync(false);

        var command = new UpdateUserPasswordCommandFaker().Generate();
        var commandResult = await _handler.Handle(command, default);
        var exceptionInResult = commandResult.Match<Exception?>(_ => null, f => f);

        commandResult.IsFaulted.Should().BeTrue();
        exceptionInResult.Should().BeOfType<UserAccountInactiveException>();
    }

    [Fact]
    public async Task Handle_IncorrectCurrentPassword_ReturnsResultWithUserCredentialsInvalidException()
    {
        var user = new UserFaker().Generate();
        _mockUserManager.SetupFindByNameAsync(user);
        _mockUserManager.SetupCheckPasswordAsync(false);

        var command = new UpdateUserPasswordCommandFaker().Generate();
        var commandResult = await _handler.Handle(command, default);
        var exceptionInResult = commandResult.Match<Exception?>(_ => null, f => f);

        commandResult.IsFaulted.Should().BeTrue();
        exceptionInResult.Should().BeOfType<UserCredentialsInvalidException>();
    }

    [Fact]
    public async Task Handle_PasswordResetFails_ReturnsResultWithUserPasswordResetFailedException()
    {
        var user = new UserFaker().Generate();
        _mockUserManager.SetupFindByNameAsync(user);
        _mockUserManager.SetupCheckPasswordAsync(true);
        _mockUserManager.SetupGeneratePasswordResetTokenAsync();
        _mockUserManager.SetupResetPasswordAsync(IdentityResult.Failed());

        var command = new UpdateUserPasswordCommandFaker().Generate();
        var commandResult = await _handler.Handle(command, default);
        var exceptionInResult = commandResult.Match<Exception?>(_ => null, f => f);

        commandResult.IsFaulted.Should().BeTrue();
        exceptionInResult.Should().BeOfType<UserPasswordResetFailedException>();
    }

    [Fact]
    public async Task Handle_PasswordUpdatedSuccessfully_ReturnsResultWithUpdatedUser()
    {
        var user = new UserFaker().Generate();
        _mockUserManager.SetupFindByNameAsync(user);
        _mockUserManager.SetupCheckPasswordAsync(true);
        _mockUserManager.SetupGeneratePasswordResetTokenAsync();
        _mockUserManager.SetupResetPasswordAsync(IdentityResult.Success);

        var command = new UpdateUserPasswordCommandFaker().Generate();
        var commandResult = await _handler.Handle(command, default);
        var userInResult = commandResult.Match(s => s, _ => null);

        commandResult.IsSuccess.Should().BeTrue();
        userInResult.Should().NotBeNull();
    }
}