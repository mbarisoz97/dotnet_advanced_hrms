using Ehrms.ProjectManagement.API.Database.Context;

namespace Ehrms.ProjectManagement.API.Handlers.Employment.Queries;

public class GetEmploymentByProjectIdQuery : IRequest<IQueryable<Database.Models.Employment>>
{
	public Guid Id { get; set; }
}

public class GetEmploymentByProjectIdQueryHandler : IRequestHandler<GetEmploymentByProjectIdQuery, IQueryable<Database.Models.Employment>>
{
	private readonly ProjectDbContext _dbContext;

	public GetEmploymentByProjectIdQueryHandler(ProjectDbContext dbContext)
	{
		_dbContext = dbContext;
	}
	public Task<IQueryable<Database.Models.Employment>> Handle(GetEmploymentByProjectIdQuery request, CancellationToken cancellationToken)
	{
		var employmentRecords = _dbContext.Employments
			.Include(x=>x.Employee)
			.AsTracking()
			.Where(x => x.ProjectId == request.Id)
			.AsQueryable();

		return Task.FromResult(employmentRecords);
	}
}