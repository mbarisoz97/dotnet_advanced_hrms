using System.Net;
using Ehrms.EmployeeInfo.API.Dtos.Title;
using Microsoft.AspNetCore.Authorization;
using Ehrms.EmployeeInfo.API.Exceptions.Title;
using Ehrms.EmployeeInfo.API.Handlers.Title.Query;
using Ehrms.EmployeeInfo.API.Handlers.Title.Command;

namespace Ehrms.EmployeeInfo.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TitleController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public TitleController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPut]
    public async Task<IActionResult> Create(CreateTitleCommand command)
    {
        var commandResult = await _mediator.Send(command);
        var actionResult = commandResult.Match(
            Succ: s => Ok(_mapper.Map<ReadTitleDto>(s)),
            Fail: this.MapTitleCreateFailureResult);

        return actionResult;
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteTitleCommand() { Id = id };
        var commandResult = await _mediator.Send(command);
        var actionResult = commandResult.Match(
            Succ: s => Ok(),
            Fail: this.MapTitleDeleteFailureResult);

        return actionResult;
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateTitleCommand command)
    {
        var commandResult = await _mediator.Send(command);
        var actionResult = commandResult.Match(
            Succ: s => Ok(_mapper.Map<ReadTitleDto>(s)),
            Fail: this.MapTitleUpdateFailureResult);

        return actionResult;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTitles()
    {
        var query = new GetAllTitlesQuery();
        var titles = await _mediator.Send(query);
        var titleDtos = _mapper.ProjectTo<ReadTitleDto>(titles);

        return Ok(titleDtos);
    }
}

internal static class TitleControllerResultMappingExtensions
{
    internal static IActionResult MapTitleCreateFailureResult(this TitleController controller, Exception err)
    {
        return err switch
        {
            TitleNameInUseException => controller.Conflict(err.Message),
            _ => controller.Problem(statusCode: (int)HttpStatusCode.InternalServerError)
        };
    }

    internal static IActionResult MapTitleDeleteFailureResult(this TitleController controller, Exception err)
    {
        return err switch
        {
            TitleNotFoundException => controller.BadRequest(err.Message),
            _ => controller.Problem(statusCode: (int)HttpStatusCode.InternalServerError)
        };
    }

    internal static IActionResult MapTitleUpdateFailureResult(this TitleController controller, Exception err)
    {
        return err switch
        {
            TitleNotFoundException => controller.BadRequest(err.Message),
            _ => controller.Problem(statusCode: (int)HttpStatusCode.InternalServerError)
        };
    }
}