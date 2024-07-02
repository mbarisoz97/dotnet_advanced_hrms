using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Ehrms.Administration.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentController : Controller
{
	private readonly IMediator _mediator;
	private readonly IMapper _mapper;

	public PaymentController(IMediator mediator, IMapper mapper)
    {
		_mediator = mediator;
		_mapper = mapper;
	}

	[HttpPut]
	public async Task Create()
	{
	}
}
