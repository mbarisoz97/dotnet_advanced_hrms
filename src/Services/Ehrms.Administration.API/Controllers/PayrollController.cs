using AutoMapper;
using Ehrms.Administration.API.Handlers.Payroll.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ehrms.Administration.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PayrollController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public PayrollController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpPut]
	public async Task<IActionResult> Create(CreatePayrollCommand command)
	{
        return Ok(await _mediator.Send(command));
	}
}