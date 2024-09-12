using Ehrms.EmployeeInfo.API.Controllers;
using Ehrms.Shared.TestHepers.Mock;
using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ehrms.EmployeeInfo.API.UnitTests.Controllers;

public class EmployeeControllerTests
{
    private readonly MockMediatr _mockMediatr = new();
    private readonly EmployeeController _employeeController;

    public EmployeeControllerTests()
    {
        var mapper = MapperFactory.CreateWithExistingProfiles();
        _employeeController = new EmployeeController(mapper, _mockMediatr.Object);
    }

    [Fact]
    public async Task Update_CommandExecutionFailedDueToTitleNotFoundException_ReturnsBadRequestObjectResult()
    {
        var command = new UpdateEmployeeCommandFaker()
            .WithId(Guid.NewGuid())
            .WithTitleId(Guid.NewGuid())
            .WithSkills(Array.Empty<Guid>())
            .Generate();
        
        var commandResult = new Result<Database.Models.Employee>(
            new TitleNotFoundException());
        
        _mockMediatr.SetupSend(command, commandResult);

        var actionResult = await _employeeController.Update(command);
        actionResult.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Update_CommandExecutionFailedDueToEmployeeNotFoundException_ReturnsBadRequestObjectResult()
    {
        var command = new UpdateEmployeeCommandFaker()
            .WithId(Guid.NewGuid())
            .WithTitleId(Guid.NewGuid())
            .WithSkills(Array.Empty<Guid>())
            .Generate();

        var commandResult = new Result<Database.Models.Employee>(
            new EmployeeNotFoundException());

        _mockMediatr.SetupSend(command, commandResult);

        var actionResult = await _employeeController.Update(command);
        actionResult.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Update_CommandExecutionFailedDueToUnhandledException_ReturnsInternalServerError()
    {
        var command = new UpdateEmployeeCommandFaker()
            .WithId(Guid.NewGuid())
            .WithTitleId(Guid.NewGuid())
            .WithSkills(Array.Empty<Guid>())
            .Generate();

        var commandResult = new Result<Database.Models.Employee>(
            new Exception());

        _mockMediatr.SetupSend(command, commandResult);

        var actionResult = await _employeeController.Update(command);
        actionResult.Should().BeOfType<ObjectResult>();
        (actionResult as ObjectResult)!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}