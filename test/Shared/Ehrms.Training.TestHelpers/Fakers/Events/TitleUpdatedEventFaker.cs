using Ehrms.Contracts.Title;

namespace Ehrms.Training.TestHelpers.Fakers.Events;

public sealed class TitleUpdatedEventFaker : Faker<TitleUpdatedEvent>
{
    public TitleUpdatedEventFaker()
    {
        RuleFor(e => e.Id, f => f.Random.Guid());
        RuleFor(e => e.Name, f => f.Random.Words(2));
    }

    public TitleUpdatedEventFaker WithId(Guid id)
    {
        RuleFor(e => e.Id, id);
        return this;
    }

    public TitleUpdatedEventFaker WithName(string name)
    {
        RuleFor(e => e.Name, name);
        return this;
    }
}