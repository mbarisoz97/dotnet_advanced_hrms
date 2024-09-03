using System.Net;
using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;
using Ehrms.Shared.TestHepers.Mock;
using Ehrms.EmployeeInfo.API.Controllers;
using Ehrms.EmployeeInfo.API.Exceptions.Title;
using Ehrms.EmployeeInfo.TestHelpers.Faker.Title.Model;
using Ehrms.EmployeeInfo.TestHelpers.Faker.Title.Command;
using Ehrms.EmployeeInfo.API.Handlers.Title.Command;
using MassTransit.NewIdProviders;

namespace Ehrms.EmployeeInfo.API.UnitTests.Controllers;

public class TitleControllerTests
{
    private readonly MockMediatr _mockMediator = new();
    private readonly TitleController _controller;

    public TitleControllerTests()
    {
        var mapper = MapperFactory.CreateWithExistingProfiles();
        _controller = new(_mockMediator.Object, mapper);
    }

    #region Create

    [Fact]
    public async Task Create_SuccessfullyExecutedCommand_ReturnsOkWithObjectResult()
    {
        var title = new TitleFaker().Generate();
        var command = new CreatTitleCommandFaker().Generate();
        var commandResult = new Result<Database.Models.Title>(title);
        _mockMediator.SetupSend(command, commandResult);

        var actionResult = await _controller.Create(command);
        actionResult.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Create_CommandExecutionFailedDueToTitleNameInUseException_ReturnConflictObjectResult()
    {
        var command = new CreatTitleCommandFaker().Generate();
        var commandResult = new Result<Database.Models.Title>(new TitleNameInUseException());
        _mockMediator.SetupSend(command, commandResult);

        var actionResult = await _controller.Create(command);
        actionResult.Should().BeOfType<ConflictObjectResult>();
    }

    [Fact]
    public async Task Create_CommandExecutionFailedDueToUnhandledException_ReturnsObjectResultWithInternalServerError()
    {
        var command = new CreatTitleCommandFaker().Generate();
        var commandResult = new Result<Database.Models.Title>(new Exception());
        _mockMediator.SetupSend(command, commandResult);

        var actionResult = await _controller.Create(command);
        actionResult.Should().BeOfType<ObjectResult>();
        (actionResult as ObjectResult)!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }

    #endregion

    #region Delete

    [Fact]
    public async Task Delete_SuccessfullyExecutedCommand_ReturnsOkResult()
    {
        var commandResult = new Result<Guid>(Guid.NewGuid());
        _mockMediator.SetupSend<DeleteTitleCommand, Result<Guid>>(commandResult);

        var actionResult = await _controller.Delete(Guid.NewGuid());
        actionResult.Should().BeOfType<OkResult>();
    }

    [Fact]
    public async Task Delete_CommandExceutionFailedDueToTitleNotFoundException_ReturnsBadRequestObjectResult()
    {
        var commandResult = new Result<Guid>(new TitleNotFoundException());
        _mockMediator.SetupSend<DeleteTitleCommand, Result<Guid>>(commandResult);

        var actionResult = await _controller.Delete(Guid.NewGuid());
        actionResult.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Delete_CommandExceutionFailedDueToUnhandledException_ReturnsObjectResultWithInternalServerError()
    {
        var command = new DeleteTitleCommandFaker().Generate();
        var commandResult = new Result<Guid>(new Exception());
        _mockMediator.SetupSend(command, commandResult);

        var actionResult = await _controller.Delete(command.Id);
        actionResult.Should().BeOfType<ObjectResult>();
        (actionResult as ObjectResult)!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }

    #endregion

    #region Update

    [Fact]
    public async Task Update_SuccessfullyExecutedCommand_ReturnsOkResultWithReadTitleDto()
    {
        var title = new TitleFaker().Generate();
        var command = new UpdateTitleCommandFaker().Generate();
        var commandResult = new Result<Database.Models.Title>(title);
        _mockMediator.SetupSend(command, commandResult);

        var actionResult = await _controller.Update(command);
        actionResult.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Update_CommandExecutionFailedDueToTitleNotFoundException_ReturnsBadRequestObjectResult()
    {
        var command = new UpdateTitleCommandFaker().Generate();
        var commandResult = new Result<Database.Models.Title>(new TitleNotFoundException());
        _mockMediator.SetupSend(command, commandResult);

        var actionResult = await _controller.Update(command);
        actionResult.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Update_CommandExeceutionFailedDueToUnhandledException_ReturnsObjectResultWithInternalServerError()
    {
        var command = new UpdateTitleCommandFaker().Generate();
        var commandResult = new Result<Database.Models.Title>(new Exception());
        _mockMediator.SetupSend(command, commandResult);

        var actionResult = await _controller.Update(command);
        actionResult.Should().BeOfType<ObjectResult>();
        (actionResult as ObjectResult)!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }

    #endregion
}