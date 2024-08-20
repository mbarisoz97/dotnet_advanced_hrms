using System.Net;
using Ehrms.Shared;
using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;
using Ehrms.Authentication.API.Exceptions;
using Ehrms.Authentication.API.Controllers;
using Ehrms.Authentication.TestHelpers.Faker.Models;
using Ehrms.Authentication.API.UnitTests.TestHelpers.Mock;

namespace Ehrms.Authentication.API.UnitTests.Controllers;

public class AccountControllerTests
{
    private readonly MockMediatr _mockMediatr = new();
    private readonly AccountController _accountController;

    public AccountControllerTests()
    {
        _accountController = new AccountController(_mockMediatr.Object);
    }

    [Fact]
    public async Task Authenticate_SuccessfullyExecutedCommand_ReturnsOkObjectResult()
    {
        var loginResponse = new GenerateTokenResponseFaker().Generate();
        var commandResult = new Result<GenerateTokenResponse?>(loginResponse);
        var command = new AuthenticateUserCommandFaker().Generate();
        _mockMediatr.SetupSend(command, commandResult);

        var actionResult = await _accountController.Authenticate(command);
        actionResult.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Authenticate_CommandFailedDueToUserInactiveException_ReturnsForbidden()
    {
        var commandResult = new Result<GenerateTokenResponse?>(new UserAccountInactiveException());
        var command = new AuthenticateUserCommandFaker().Generate();
        _mockMediatr.SetupSend(command, commandResult);

        var actionResult = await _accountController.Authenticate(command);
        actionResult.Should().BeOfType<ObjectResult>();
        (actionResult as ObjectResult)!.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Authenticate_CommandFailedDueToInvalidPasswordException_ReturnsBadRequest()
    {
        var commandResult = new Result<GenerateTokenResponse?>(new UserCredentialsInvalidException());
        var command = new AuthenticateUserCommandFaker().Generate();
        _mockMediatr.SetupSend(command, commandResult);

        var actionResult = await _accountController.Authenticate(command);
        actionResult.Should().BeOfType<BadRequestObjectResult>();
    }
    
    [Fact]
    public async Task Refresh_SuccessfullyExecutedCommand_ReturnsOkObjectResult()
    {
        var loginResponse = new GenerateTokenResponseFaker().Generate();
        var commandResult = new Result<GenerateTokenResponse?>(loginResponse);
        var command = new RefreshAuthenticationCommandFaker().Generate();
        _mockMediatr.SetupSend(command, commandResult);

        var actionResult = await _accountController.Refresh(command);
        actionResult.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Refresh_CommandExecutionFailedDueTooUserInactiveException_ReturnsForbidden()
    {
        var commandResult = new Result<GenerateTokenResponse?>(new UserAccountInactiveException());
        var command = new RefreshAuthenticationCommandFaker().Generate();
        _mockMediatr.SetupSend(command, commandResult);

        var actionResult = await _accountController.Refresh(command);
        actionResult.Should().BeOfType<ObjectResult>();
        (actionResult as ObjectResult)!.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
    }
}