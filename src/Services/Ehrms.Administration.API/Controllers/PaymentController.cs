using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Ehrms.Administration.API.Handlers.Payment.Commands;

namespace Ehrms.Administration.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentController : Controller
{
	private readonly IMapper _mapper;
	private readonly IMediator _mediator;

	public PaymentController(IMapper mapper, IMediator mediator)
    {
		_mapper = mapper;
		_mediator = mediator;
	}

    [HttpPut]
	public async Task<IActionResult> Create([FromBody] CreatePaymentCommand createPaymentCommand)
	{
		var id = await _mediator.Send(createPaymentCommand);
		return Ok(id);
	}
}