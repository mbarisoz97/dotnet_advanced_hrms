using Ehrms.EmployeeInfo.API.Dtos.Title;
using Microsoft.AspNetCore.Authorization;
using Ehrms.EmployeeInfo.API.Exceptions.Title;
using Ehrms.EmployeeInfo.API.Handlers.Title.Command;
using System.Net;

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
}