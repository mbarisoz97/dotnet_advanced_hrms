using Ehrms.ProjectManagement.API.Handlers.Employment.Queries;

namespace Ehrms.ProjectManagement.API.UnitTests.TestHelpers.Faker.Command;

internal class GetEmploymentByEmployeeIdQueryFaker : Faker<GetEmploymentByEmployeeIdQuery>
{
	internal GetEmploymentByEmployeeIdQueryFaker WithId(Guid id)
	{
		RuleFor(x => x.Id, id);
		return this;
	}
}