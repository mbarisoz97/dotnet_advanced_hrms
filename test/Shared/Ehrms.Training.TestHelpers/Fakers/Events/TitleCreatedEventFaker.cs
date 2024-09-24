using Ehrms.Contracts.Title;

namespace Ehrms.Training.TestHelpers.Fakers.Events;

public sealed class TitleCreatedEventFaker : Faker<TitleCreatedEvent>
{
    public TitleCreatedEventFaker()
    {
        RuleFor(e => e.Id, f => f.Random.Guid());
        RuleFor(e => e.Name, f => f.Random.Words(2));
    }

    public TitleCreatedEventFaker WithName(string name)
    {
        RuleFor(e => e.Name, name);
        return this;
    }
}