using Ehrms.Contracts.Employee;

namespace Ehrms.TrainingManagement.API.UnitTests.TestHelpers.Fakers.Events;

internal class EmployeeDeletedEventFaker : Faker<EmployeeDeletedEvent>
{
    public EmployeeDeletedEventFaker()
    {
		RuleFor(e => e.Id, Guid.NewGuid());
	}

    public EmployeeDeletedEventFaker WithId(Guid id)
    {
		RuleFor(e => e.Id, id);
        return this;
	}
}