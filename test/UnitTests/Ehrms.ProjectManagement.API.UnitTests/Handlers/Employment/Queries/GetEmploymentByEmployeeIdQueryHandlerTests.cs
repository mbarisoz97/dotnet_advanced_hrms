using Ehrms.ProjectManagement.API.Handlers.Employment.Queries;

namespace Ehrms.ProjectManagement.API.UnitTests.Handlers.Employment.Queries;

public class GetEmploymentByEmployeeIdQueryHandlerTests
{
	private readonly GetEmploymentByEmployeeIdQueryHandler _handler;
	private readonly ProjectDbContext _projectDbContext;

	public GetEmploymentByEmployeeIdQueryHandlerTests()
	{
		_projectDbContext = new(new DbContextOptionsBuilder()
			.UseInMemoryDatabase(Guid.NewGuid().ToString())
			.Options);

		_handler = new(_projectDbContext);
	}

	[Fact]
	public async Task Handle_EmployeeWithEmploymentRecords_ReturnsExpectedEmploymentRecords()
	{
		var employee = new EmployeeFaker().Generate();
		await _projectDbContext.AddAsync(employee);

		var project = new ProjectFaker().Generate();
		await _projectDbContext.AddAsync(project);

		var employmentRecords = new List<Database.Models.Employment>()
		{
			//Active employment
			new EmploymentFaker()
				.WithEmployee(employee)
				.WithProject(project)
				.Generate(),

			//Inactive employment
			new EmploymentFaker()
				.WithEmployee(employee)
				.WithProject(project)
				.WithEndDate()
				.Generate()
		};

		await _projectDbContext.AddRangeAsync(employmentRecords);
		await _projectDbContext.SaveChangesAsync();

		var query = new GetEmploymentByEmployeeIdQueryFaker()
			.WithId(employee.Id)
			.Generate();

		var returnedEmploymentRecords = await _handler.Handle(query, default);

		returnedEmploymentRecords.Should().HaveCount(employmentRecords.Count);
		returnedEmploymentRecords.Should().BeEquivalentTo(employmentRecords);
	}
}