namespace Ehrms.Training.TestHelpers.Fakers.Events;

public sealed class EmployeeUpdatedEventFaker : Faker<EmployeeUpdatedEvent>
{
	public EmployeeUpdatedEventFaker()
	{
		RuleFor(e => e.Id, Guid.NewGuid());
		RuleFor(e => e.FirstName, f => f.Name.FirstName());
		RuleFor(e => e.FirstName, f => f.Name.LastName());
	}

	public EmployeeUpdatedEventFaker WithId(Guid id)
	{
		RuleFor(e => e.Id, id);
		return this;
	}
}