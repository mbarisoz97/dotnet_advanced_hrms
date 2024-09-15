using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Ehrms.Administration.API.Dto.PaymentCategorty;
using Ehrms.Administration.API.Handlers.PaymentCategory.Queries;
using Ehrms.Administration.API.Handlers.PaymentCategory.Commands;

namespace Ehrms.Administration.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentCategoryController : Controller
{
	private readonly IMediator _mediator;
	private readonly IMapper _mapper;

	public PaymentCategoryController(IMediator mediator, IMapper mapper)
	{
		_mediator = mediator;
		_mapper = mapper;
	}

	[HttpPut]
	public async Task<IActionResult> Create(CreatePaymentCategoryCommand createPaymentCategoryCommand)
	{
		var categoryId = await _mediator.Send(createPaymentCategoryCommand);
		return Ok(categoryId);
	}

	[HttpPost]
	public async Task<IActionResult> Update(UpdatePaymentCategoryCommand updatePaymentCategoryCommand)
	{
		var paymentCategory = await _mediator.Send(updatePaymentCategoryCommand);
		var readPaymentCategoryDto = _mapper.Map<ReadPaymentCategoryDto>(paymentCategory);

		return Ok(readPaymentCategoryDto);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> Get(Guid id)
	{
		var query = new GetPaymentCategoryQuery() { Id = id };
		var category = await _mediator.Send(query);
		var readCategoryDto = _mapper.Map<ReadPaymentCategoryDto>(category);
		
		return Ok(readCategoryDto);
	}

	[HttpGet]
	public async Task<IActionResult> Get()
	{
		var query = new GetAllPaymentCategoriesQuery();
		var category = await _mediator.Send(query);
		var readCategoryDto = _mapper.ProjectTo<ReadPaymentCategoryDto>(category);

		return Ok(readCategoryDto);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(Guid id)
	{
		var command = new DeletePaymentCategoryCommand() { Id = id };
		await _mediator.Send(command);

		return NoContent();
	}
}