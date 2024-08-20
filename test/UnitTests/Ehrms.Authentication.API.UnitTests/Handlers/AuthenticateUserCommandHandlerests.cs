using Ehrms.Shared;
using Ehrms.Authentication.API.Exceptions;
using Ehrms.Authentication.API.Database.Context;
using Ehrms.Authentication.TestHelpers.Faker.Models;
using Ehrms.Authentication.API.UnitTests.TestHelpers;
using Ehrms.Authentication.API.Handlers.Auth.Commands;
using Ehrms.Authentication.API.UnitTests.TestHelpers.Mock;

namespace Ehrms.Authentication.API.UnitTests.Handlers;
public class AuthenticateUserCommandHandlerests
{
    private readonly MockUserManager _mockUserManager;
    private readonly ApplicationUserDbContext _dbContext;
    private readonly AuthenticateUserCommandHandler _handler;
    private Mock<ITokenHandler> _mockTokenHandler = new();

    public AuthenticateUserCommandHandlerests()
    {
        _dbContext = TestDbContextFactory.CreateDbContext(nameof(AuthenticateUserCommandHandlerests));
        _mockUserManager = new(_dbContext);
        var mapper = MapperFactory.CreateWithExistingProfiles();
        _handler = new(_dbContext, _mockUserManager.Object, _mockTokenHandler.Object, mapper);
    }

    [Fact]
    public async Task Handle_NonExistingUsername_ReturnsResultWithUserNotFoundException()
    {
        _mockUserManager.SetupFindByNameAsync();
        var command = new AuthenticateUserCommand();

        var result = await _handler.Handle(command, default);
        var exceptionResult = result.Match<Exception?>(s => null, f => f);

        result.IsSuccess.Should().BeFalse();
        exceptionResult.Should().BeOfType<UserNotFoundException>();
    }

    [Fact]
    public async Task Handle_ExistingUserWithWrongPassword_ReturnsResultWithWrongCredentialsException()
    {
        var user = new UserFaker().Generate();
        _mockUserManager.SetupFindByNameAsync(user);
        _mockUserManager.SetupCheckPasswordAsync(isPasswordTrue: false);

        var command = new AuthenticateUserCommand();
        var result = await _handler.Handle(command, default);
        var exceptionResult = result.Match<Exception?>(s => null, f => f);

        result.IsSuccess.Should().BeFalse();
        exceptionResult.Should().BeOfType<UserCredentialsInvalidException>();
    }

    [Fact]
    public async Task Handle_SuccessfullTokenGeneration_UpdatesUser()
    {
        var user = new UserFaker().Generate();

        _mockUserManager.SetupFindByNameAsync(user);
        _mockUserManager.SetupCheckPasswordAsync(isPasswordTrue: true);

        var mockTokenResponse = new GenerateTokenResponseFaker()
            .Generate();

        _mockTokenHandler.Setup(x => x.Generate(It.IsAny<GenerateJwtRequest>()))
            .Returns(mockTokenResponse);

        var command = new AuthenticateUserCommandFaker().Generate();
        var result = await _handler.Handle(command, default);
        var generateTokenResponse = result.Match(s => s, f => null);

        result.IsSuccess.Should().BeTrue();
        user.RefreshToken.Should().Be(generateTokenResponse?.RefreshToken);
        user.RefreshTokenExpiry.Should().BeAfter(DateTime.UtcNow);
    }

    [Fact]
    public async Task Handle_ExistingUserWithValidCredentials_ReturnsResultWithAccessTokens()
    {
        var user = new UserFaker().Generate();

        _mockUserManager.SetupFindByNameAsync(user);
        _mockUserManager.SetupCheckPasswordAsync(isPasswordTrue: true);

        var mockTokenResponse = new GenerateTokenResponseFaker().Generate();
        _mockTokenHandler.Setup(x => x.Generate(It.IsAny<GenerateJwtRequest>()))
            .Returns(mockTokenResponse);

        var command = new AuthenticateUserCommandFaker().Generate();
        var result = await _handler.Handle(command, default);
        var generateTokenResponse = result.Match(s => s, f => null);

        result.IsSuccess.Should().BeTrue();
        generateTokenResponse.Should().BeEquivalentTo(mockTokenResponse);
    }
    
    [Fact]
    public async Task Handle_ExistingUserWithInactiveAccount_ReturnsResultWithInactiveAccountException()
    {
        var user = new UserFaker()
            .WithAccountStatus(isActive: false)
            .Generate();

        _mockUserManager.SetupFindByNameAsync(user);
        _mockUserManager.SetupCheckPasswordAsync(isPasswordTrue: true);

        var mockTokenResponse = new GenerateTokenResponseFaker().Generate();
        _mockTokenHandler.Setup(x => x.Generate(It.IsAny<GenerateJwtRequest>()))
            .Returns(mockTokenResponse);

        var command = new AuthenticateUserCommandFaker().Generate();
        var result = await _handler.Handle(command, default);
        var exceptionResult = result.Match<Exception?>(s => null, f => f);

        result.IsSuccess.Should().BeFalse();
        exceptionResult.Should().BeOfType<UserAccountInactiveException>();
    }
}