﻿using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Ehrms.Administration.API.Dto.Payment;
using Ehrms.Administration.API.Handlers.Payment.Commands;
using Ehrms.Administration.API.Handlers.Payment.Queries;

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

	[HttpPost]
	public async Task<IActionResult> Update([FromBody] UpdatePaymentCommand updatePaymentCommand)
	{
		var paymentRecord = await _mediator.Send(updatePaymentCommand);
		var paymentDto = _mapper.Map<ReadPaymentDto>(paymentRecord);

		return Ok(paymentDto);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(Guid id)
	{
		await _mediator.Send(new DeletePaymentCommand()
		{
			Id = id
		});

		return NoContent();
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> Get(Guid id)
	{
		var payment = await _mediator.Send(new GetPaymentQuery
		{
			Id = id
		});
		var readPaymentDto = _mapper.Map<ReadPaymentDto>(payment);

		return Ok(readPaymentDto);
	}

	[HttpGet]
	public async Task<IActionResult> Get()
	{
		var payment = await _mediator.Send(new GetAllPaymentsQuery());
		var readPaymentDto = _mapper.ProjectTo<ReadPaymentDto>(payment);

		return Ok(readPaymentDto);
	}
}