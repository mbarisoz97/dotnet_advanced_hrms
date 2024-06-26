﻿using Microsoft.AspNetCore.Authorization;

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
        var employee = await _mediator.Send(createEmployeeCommand);
        var readEmployeeDto = _mapper.Map<ReadEmployeeDto>(employee);

        return Ok(readEmployeeDto);
    }

    [HttpPost]
    public async Task<IActionResult> Update([FromBody] UpdateEmployeeCommand updateEmployeeCommand)
    {
        var employee = await _mediator.Send(updateEmployeeCommand);
        var readEmployeeDto = _mapper.Map<ReadEmployeeDto>(employee);

        return Ok(readEmployeeDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteEmployeeCommand() { Id = id };
        await _mediator.Send(command);

        return Ok();
    }
}