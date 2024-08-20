using Ehrms.Shared;
using Ehrms.Authentication.API.Exceptions;
using Ehrms.Authentication.API.Database.Context;
using Ehrms.Authentication.TestHelpers.Faker.Models;
using Ehrms.Authentication.API.UnitTests.TestHelpers;
using Ehrms.Authentication.API.Handlers.Auth.Commands;
using Ehrms.Authentication.API.UnitTests.TestHelpers.Mock;

namespace Ehrms.Authentication.API.UnitTests.Handlers;

public class RefreshAuthenticationCommandHandlerTests
{
    private readonly MockUserManager _mockUserManager;
    private readonly MockTokenHandler _mockTokenHandler;
    private readonly ApplicationUserDbContext _dbContext;

    private readonly RefreshAuthenticationCommandHandler _handler;

    public RefreshAuthenticationCommandHandlerTests()
    {
        _dbContext = TestDbContextFactory.CreateDbContext(nameof(RefreshAuthenticationCommandHandlerTests));
        _mockTokenHandler = new();
        _mockUserManager = new(_dbContext);

        _handler = new(_mockTokenHandler.Object, _mockUserManager.Object);
    }

    [Fact]
    public async Task Handle_InvalidUsernameInAccessToken_ReturnsResultWithInvalidAccessTokenException()
    {
        var command = new RefreshAuthenticationCommandFaker().Generate();
        var result = await _handler.Handle(command, default);
        var exceptionInResult = result.Match<Exception?>(s => null, f => f);

        result.IsFaulted.Should().BeTrue();
        exceptionInResult.Should().BeOfType<InvalidTokenException>();
    }

    [Fact]
    public async Task Handle_NonExistingUserName_ReturnsResultWithUserNotFoundException()
    {
        var user = new UserFaker().Generate();
        _mockTokenHandler.SetupGetPrincipalFromExpiredToken(user);
        _mockUserManager.SetupFindByNameAsync(null);

        var command = new RefreshAuthenticationCommandFaker().Generate();
        var result = await _handler.Handle(command, default);
        var exceptionInResult = result.Match<Exception?>(s => null, f => f);

        result.IsFaulted.Should().BeTrue();
        exceptionInResult.Should().BeOfType<UserNotFoundException>();
    }

    [Fact]
    public async Task Handle_InvalidRefreshToken_ReturnsResultWithInvalidAccessTokenException()
    {
        var user = new UserFaker().Generate();
        _mockTokenHandler.SetupGetPrincipalFromExpiredToken(user);
        _mockUserManager.SetupFindByNameAsync(user);

        var command = new RefreshAuthenticationCommandFaker()
            .WithRefreshToken(string.Empty)
            .Generate();

        var result = await _handler.Handle(command, default);
        var exceptionInResult = result.Match<Exception?>(s => null, f => f);

        result.IsFaulted.Should().BeTrue();
        exceptionInResult.Should().BeOfType<InvalidTokenException>();
    }

    [Fact]
    public async Task Handle_TokenGenerationFails_ReturnsResultWithInvalidAccessTokenException()
    {
        var user = new UserFaker().Generate();
        var command = new RefreshAuthenticationCommandFaker();
        _mockTokenHandler.SetupGenerate();

        var result = await _handler.Handle(command, default);
        var exceptionInResult = result.Match<Exception?>(s => null, f => f);

        result.IsFaulted.Should().BeTrue();
        exceptionInResult.Should().BeOfType<InvalidTokenException>();
    }
    
    [Fact]
    public async Task Handle_UserAccountIsInactive_ReturnsResultWithInvalidAccessTokenException()
    {
        var user = new UserFaker()
            .WithAccountStatus(isActive: false)
            .Generate();
        
        _mockTokenHandler.SetupGetPrincipalFromExpiredToken(user);
        _mockUserManager.SetupFindByNameAsync(user);
        _mockTokenHandler.SetupGenerate(new GenerateTokenResponse());

        var command = new RefreshAuthenticationCommandFaker();
        var result = await _handler.Handle(command, default);
        var exceptionInResult = result.Match<Exception?>(s => null, f => f);

        result.IsFaulted.Should().BeTrue();
        exceptionInResult.Should().BeOfType<UserAccountInactiveException>();
    }

    [Fact]
    public async Task Handle_TokenGenerationSuccessful_ReturnsResultWithGeneratedToken()
    {
        var role = new RoleFaker().Generate();
        var user = new UserFaker().Generate();
        var userRole = new UserRoleFaker()
            .WithRole(role)
            .WithUser(user)
            .Generate();
        user.UserRoles.Add(userRole);
        
        _mockTokenHandler.SetupGetPrincipalFromExpiredToken(user);
        _mockUserManager.SetupFindByNameAsync(user);
        _mockTokenHandler.SetupGenerate(new GenerateTokenResponse());

        var command = new RefreshAuthenticationCommandFaker()
            .WithRefreshToken(user.RefreshToken)
            .Generate();

        var result = await _handler.Handle(command, default);
        var tokenConfig = result.Match(s => s, f => null);

        result.IsSuccess.Should().BeTrue();
        tokenConfig?.RefreshToken.Should().Be(user.RefreshToken);
    }
}