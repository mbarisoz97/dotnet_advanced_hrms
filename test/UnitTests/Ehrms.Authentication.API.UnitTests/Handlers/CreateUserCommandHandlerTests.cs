using Ehrms.Authentication.API.Adapter;
using Ehrms.Authentication.TestHelpers.Faker.Models;

namespace Ehrms.Authentication.API.UnitTests.Handlers;

public class CreateUserCommandHandlerTests
{
    private readonly CreateUserCommandHandler _handler;
    private readonly Mock<IUserManagerAdapter> _userManagerMock = new();

    public CreateUserCommandHandlerTests()
    {
        _handler = new(_userManagerMock.Object);
    }

    [Fact]
    public async Task Handle_UserNameIsAlreadyInUse_ReturnsDetailsInUseExceptionResult()
    {
        var user = new UserFaker().Generate();
        _userManagerMock.Setup(x => x.Users)
            .Returns(new List<User> { user }.AsQueryable());

        var createUserCommand = new RegisterUserCommandFaker()
            .WithUserName(user.UserName!)
            .Generate();

        var result = await _handler.Handle(createUserCommand, default);
        result.IsFaulted.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_EmailIsAlreadyInUse_ReturnsDetailsInUseExceptionResult()
    {
        var user = new UserFaker().Generate();
        _userManagerMock.Setup(x => x.Users)
            .Returns(new List<User> { user }.AsQueryable());

        var createUserCommand = new RegisterUserCommandFaker()
            .WithEmail(user.Email!)
            .Generate();

        var result = await _handler.Handle(createUserCommand, default);
        result.IsFaulted.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_UserManagerFailedToRegisterUser_ReturnsCouldNotCreateUserExceptionResult()
    {
        var user = new UserFaker().Generate();
        _userManagerMock.Setup(x => x.Users)
            .Returns(new List<User> { user }.AsQueryable());

        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed()); 

        var createUserCommand = new RegisterUserCommandFaker().Generate();
        var result = await _handler.Handle(createUserCommand, default);

        result.IsFaulted.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_SuccessfullyCreatedUser_ReturnsUserWithResult()
    {
        var user = new UserFaker().Generate();
        _userManagerMock.Setup(x => x.Users)
            .Returns(new List<User> { user }.AsQueryable());

        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        var createUserCommand = new RegisterUserCommandFaker().Generate();
        var result = await _handler.Handle(createUserCommand, default);

        result.IsSuccess.Should().BeTrue();
    }
}