using Ehrms.EmployeeInfo.API.Database.Context;

namespace Ehrms.EmployeeInfo.API.Handlers.Employee.Query;

public sealed class GetEmployeeByIdQuery : IRequest<Database.Models.Employee>
{
	public Guid Id { get; set; }
}

internal sealed class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, Database.Models.Employee>
{
	private readonly EmployeeInfoDbContext _context;

	public GetEmployeeByIdQueryHandler(EmployeeInfoDbContext context)
	{
		_context = context;
	}

	public async Task<Database.Models.Employee> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
	{
		Database.Models.Employee employee = await _context.Employees
		   .Include(x => x.Skills)
		   .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
		   ?? throw new ArgumentException($"Could not find employee with id {request.Id}");

		return employee;
	}
}