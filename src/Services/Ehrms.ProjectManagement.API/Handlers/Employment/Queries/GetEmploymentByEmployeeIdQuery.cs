namespace Ehrms.ProjectManagement.API.Handlers.Employment.Queries;

public class GetEmploymentByEmployeeIdQuery : IRequest<IQueryable<Database.Models.Employment>>
{
	public Guid Id { get; set; }
}

internal class GetEmploymentByEmployeeIdQueryHandler : IRequestHandler<GetEmploymentByEmployeeIdQuery, IQueryable<Database.Models.Employment>>
{
	private readonly ProjectDbContext _projectDbContext;

	public GetEmploymentByEmployeeIdQueryHandler(ProjectDbContext projectDbContext)
	{
		_projectDbContext = projectDbContext;
	}

	public Task<IQueryable<Database.Models.Employment>> Handle(GetEmploymentByEmployeeIdQuery request, CancellationToken cancellationToken)
	{
		var employmentRecords = _projectDbContext.Employments
			.Include(x => x.Project)
			.Where(x => x.EmployeeId == request.Id)
			.AsQueryable();

		return Task.FromResult(employmentRecords);
	}
}