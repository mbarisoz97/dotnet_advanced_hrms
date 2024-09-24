namespace Ehrms.Training.TestHelpers.Fakers.Events;

public sealed class TitleFaker : Faker<Title>
{
    public TitleFaker()
    {
        RuleFor(e => e.Id, f => f.Random.Guid());
        RuleFor(e => e.Name, f => f.Random.Words(2));
    }
}