using Ehrms.Shared.TestHepers.Mock;
using Ehrms.EmployeeInfo.API.Controllers;
using Ehrms.EmployeeInfo.TestHelpers.Faker.Title.Command;
using LanguageExt.Common;
using Ehrms.EmployeeInfo.API.Exceptions.Title;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Ehrms.EmployeeInfo.TestHelpers.Faker.Title.Model;

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
    public async Task Create_CommandExecutionFailedDueToUnhandledException_ReturnConflictObjectResult()
    {
        var command = new CreatTitleCommandFaker().Generate();
        var commandResult = new Result<Database.Models.Title>(new Exception());
        _mockMediator.SetupSend(command, commandResult);

        var actionResult = await _controller.Create(command);
        actionResult.Should().BeOfType<ObjectResult>();
        (actionResult as ObjectResult)!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}