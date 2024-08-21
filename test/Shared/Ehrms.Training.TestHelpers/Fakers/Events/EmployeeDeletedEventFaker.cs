namespace Ehrms.Training.TestHelpers.Fakers.Events;

public sealed class EmployeeDeletedEventFaker : Faker<EmployeeDeletedEvent>
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