using Ehrms.Contracts.Title;

namespace Ehrms.Training.TestHelpers.Fakers.Events;

public sealed class TitleDeletedEventFaker : Faker<TitleDeletedEvent>
{
    public TitleDeletedEventFaker()
    {
        RuleFor(e => e.Id, f => f.Random.Guid());
    }

    public TitleDeletedEventFaker WithId(Guid id)
    {
        RuleFor(e=>e.Id, id);
        return this;
    }
}