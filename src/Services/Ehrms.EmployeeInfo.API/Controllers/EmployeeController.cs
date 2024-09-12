using Ehrms.EmployeeInfo.API.Exceptions.Title;
using Ehrms.EmployeeInfo.API.Handlers.Employee.Command;
using Microsoft.AspNetCore.Authorization;
using System.Net;

namespace Ehrms.EmployeeInfo.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EmployeeController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public EmployeeController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var query = new GetEmployeesQuery();
        var employeeCollection = await _mediator.Send(query);
        var employeeReadDtoCollection = _mapper.ProjectTo<ReadEmployeeDto>(employeeCollection);

        return Ok(employeeReadDtoCollection);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var query = new GetEmployeeByIdQuery() { Id = id };
        var employee = await _mediator.Send(query);
        var readEmployeeDto = _mapper.Map<ReadEmployeeDto>(employee);

        return Ok(readEmployeeDto);
    }

    [HttpPut]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeCommand createEmployeeCommand)
    {
        var commandResult = await _mediator.Send(createEmployeeCommand);
        var actionResult = commandResult.Match(
            Succ: e => Ok(_mapper.Map<ReadEmployeeDto>(e)),
            Fail: this.MapEmployeeCreateFailureResult);

        return actionResult;
    }

    [HttpPost]
    public async Task<IActionResult> Update([FromBody] UpdateEmployeeCommand updateEmployeeCommand)
    {
        var commandResult = await _mediator.Send(updateEmployeeCommand);
        var actionResult = commandResult.Match(
            Succ: e => Ok(_mapper.Map<ReadEmployeeDto>(e)),
            Fail: this.MapEmployeeUpdateFailureResult);

        return actionResult;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteEmployeeCommand() { Id = id };
        await _mediator.Send(command);

        return Ok();
    }
}

internal static class EmployeeControllerResultMappingExtensions
{
    internal static IActionResult MapEmployeeCreateFailureResult(this EmployeeController controller, Exception err)
    {
        if (err is TitleNotFoundException)
        {
            return controller.BadRequest(err.Message);
        }
        return controller.Problem(statusCode: (int)HttpStatusCode.InternalServerError);
    }

    internal static IActionResult MapEmployeeUpdateFailureResult(this EmployeeController controller, Exception err)
    {
        return err switch
        {
            TitleNotFoundException or 
            EmployeeNotFoundException => controller.BadRequest(err.Message),
            _ => controller.Problem(statusCode: (int)HttpStatusCode.InternalServerError)
        };
    }
}