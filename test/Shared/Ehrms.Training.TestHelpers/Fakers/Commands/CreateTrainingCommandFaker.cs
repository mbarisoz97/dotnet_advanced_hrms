namespace Ehrms.Training.TestHelpers.Fakers.Commands;

public sealed class CreateTrainingCommandFaker : Faker<CreateTrainingCommand>
{
    public CreateTrainingCommandFaker()
    {
        RuleFor(e => e.Name, f => f.Random.Word());
        RuleFor(e => e.Description, f => f.Random.Words());
        RuleFor(e => e.PlannedAt, DateTime.UtcNow);
        RuleFor(e => e.StartsAt, DateTime.UtcNow.AddHours(1));
        RuleFor(e => e.EndsAt, DateTime.UtcNow.AddHours(2));
    }

    public CreateTrainingCommandFaker WithStartDate(DateTime startTime)
    {
        RuleFor(e => e.StartsAt, startTime);
        return this;
    }
    
    public CreateTrainingCommandFaker WithEndDate(DateTime startTime)
    {
        RuleFor(e => e.EndsAt, startTime);
        return this;
    }
}