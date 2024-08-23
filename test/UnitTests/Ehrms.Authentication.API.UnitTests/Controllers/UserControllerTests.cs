using System.Net;
using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;
using Ehrms.Authentication.API.Controllers;
using Ehrms.Authentication.API.Handlers.User.Queries;

namespace Ehrms.Authentication.API.UnitTests.Controllers;

public class UserControllerTests
{
    private readonly UserController _userController;
    private readonly MockMediatr _mockMediatr = new();

    public UserControllerTests()
    {
        var mapper = MapperFactory.CreateWithExistingProfiles();
        _userController = new UserController(_mockMediatr.Object, mapper);
    }

    [Fact]
    public async Task Register_SuccessfullyExecutedCommand_ReturnsOkObjectResult()
    {
        var command = new RegisterUserCommandFaker().Generate();
        var user = new UserFaker().Generate();
        _mockMediatr.SetupSend(command, new Result<User>(user));

        var actionResult = await _userController.Register(command);
        actionResult.Should().BeOfType<OkObjectResult>();
    }
    
    [Fact]
    public async Task Register_CommandExecutionFailed_ReturnsBadRequestResult()
    {
        var command = new RegisterUserCommandFaker().Generate();
        var commandResult = new Result<User>(new Exception());    
        
        _mockMediatr.SetupSend(command, commandResult);
        
        var actionResult = await _userController.Register(command);
        actionResult.Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public async Task Update_SuccessfullyExecutedCommand_ReturnsOkObjectResult()
    {
        var user = new UserFaker().Generate();
        var command = new UpdateUserCommandFaker().Generate();
        var commandResult = new Result<User>(user);    
        
        _mockMediatr.SetupSend(command, commandResult);
        
        var actionResult = await _userController.Update(command);
        actionResult.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Update_CommandExecutionFailedDueToException_ReturnsBadRequestResult()
    {
        var command = new UpdateUserCommandFaker().Generate();
        var commandResult = new Result<User>(new Exception());    
        
        _mockMediatr.SetupSend(command, commandResult);
        
        var actionResult = await _userController.Update(command);
        actionResult.Should().BeOfType<BadRequestResult>(); 
    }
    
    [Fact]
    public async Task Update_CommandExecutionFailedWithUserUpdateNotAllowedException_ReturnsUnauthorizedResult()
    {
        var command = new UpdateUserCommandFaker().Generate();
        var commandResult = new Result<User>(new UserUpdateNotAllowedException());    
        
        _mockMediatr.SetupSend(command, commandResult);
        
        var actionResult = await _userController.Update(command);
        actionResult.Should().BeOfType<UnauthorizedResult>();
    }

    [Fact]
    public async Task GetUserById_SuccessfullyExecutedQuery_ReturnsOkObjectResult()
    {
        var user = new UserFaker().Generate();
        var query = new GetUserByIdQuery() { Id = user.Id };
        var queryResult = new Result<User>(user);    
        
        _mockMediatr.SetupSend<GetUserByIdQuery, Result<User>>(queryResult);
        
        var actionResult = await _userController.GetUserById(Guid.Empty);
        actionResult.Should().BeOfType<OkObjectResult>(); 
    }
    
    [Fact]
    public async Task GetUserById_FailedResult_ReturnsBadRequestResult()
    {
        var query = new GetUserByIdQuery() { Id = Guid.Empty };
        var queryResult = new Result<User>(new Exception());    
        
        _mockMediatr.SetupSend(query, queryResult);
        
        var actionResult = await _userController.GetUserById(query.Id);
        actionResult.Should().BeOfType<BadRequestResult>(); 
    }

    [Fact]
    public async Task GetUsers_SuccessfulQueryResult_ReturnsBadRequestResult()
    {
        var userCollection = Enumerable.Empty<User>().AsQueryable();
        _mockMediatr.SetupSend<GetUsersQuery, IQueryable<User>>(userCollection);
        
        var actionResult = await _userController.GetUsers();
        actionResult.Should().BeOfType<OkObjectResult>(); 
    }

    [Fact]
    public async Task ResetPassword_CommandExecutionFailedDueToUserNotFoundException_ReturnsNotFoundObjectResult()
    {
        var command = new UpdateUserPasswordCommandFaker().Generate();
        var commandResult = new Result<User?>(new UserNotFoundException());
        
        _mockMediatr.SetupSend(command, commandResult);
        
        var actionResult = await _userController.ResetPassword(command);
        actionResult.Should().BeOfType<NotFoundObjectResult>(); 
    }
    
    [Fact]
    public async Task ResetPassword_CommandExecutionFailedDueToUserAccountInactiveException_ReturnsUnauthorizedObjectResult()
    {
        var command = new UpdateUserPasswordCommandFaker().Generate();
        var commandResult = new Result<User?>(new UserAccountInactiveException());
        
        _mockMediatr.SetupSend(command, commandResult);
        
        var actionResult = await _userController.ResetPassword(command);
        actionResult.Should().BeOfType<UnauthorizedObjectResult>(); 
    }
    
    [Fact]
    public async Task ResetPassword_CommandExecutionFailedDueToUserCredentialsInvalidException_ReturnsBadRequestObjectResult()
    {
        var command = new UpdateUserPasswordCommandFaker().Generate();
        var commandResult = new Result<User?>(new UserCredentialsInvalidException());
        
        _mockMediatr.SetupSend(command, commandResult);
        
        var actionResult = await _userController.ResetPassword(command);
        actionResult.Should().BeOfType<BadRequestObjectResult>(); 
    }
    
    [Fact]
    public async Task ResetPassword_CommandExecutionFailedDueToUserPasswordResetFailedException_ReturnsBadRequestObjectResult()
    {
        var command = new UpdateUserPasswordCommandFaker().Generate();
        var commandResult = new Result<User?>(new UserPasswordResetFailedException());
        
        _mockMediatr.SetupSend(command, commandResult);
        
        var actionResult = await _userController.ResetPassword(command);
        actionResult.Should().BeOfType<BadRequestObjectResult>(); 
    }
    
    [Fact]
    public async Task ResetPassword_CommandExecutionFailedDueToUnhandledExceptionResult_ReturnsInternalServerError()
    {
        var command = new UpdateUserPasswordCommandFaker().Generate();
        var commandResult = new Result<User?>(new Exception());
        
        _mockMediatr.SetupSend(command, commandResult);
        
        var actionResult = await _userController.ResetPassword(command);
        actionResult.Should().BeOfType<ObjectResult>();
        (actionResult as ObjectResult)!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}