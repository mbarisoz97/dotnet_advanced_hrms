

namespace Ehrms.Training.TestHelpers.Fakers.Commands;

public sealed class CreateTrainingCommandFaker : Faker<CreateTrainingCommand>
{
    public CreateTrainingCommandFaker()
    {
        RuleFor(e => e.Name, f => f.Random.Word());
        RuleFor(e => e.PlannedAt, f => f.Date.Future());
        RuleFor(e => e.Description, f => f.Random.Words());
    }
}