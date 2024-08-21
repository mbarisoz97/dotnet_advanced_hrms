namespace Ehrms.Training.TestHelpers.Fakers.Commands;

public sealed class UpdateTrainingCommandFaker : Faker<UpdateTrainingCommand>
{
    public UpdateTrainingCommandFaker()
    {
        RuleFor(e => e.Name, f => f.Random.Word());
        RuleFor(e => e.StartsAt, DateTime.Today.AddHours(1));
        RuleFor(e => e.EndsAt, DateTime.Today.AddHours(2));
        RuleFor(e => e.Description, f => f.Random.Words());
    }

    public UpdateTrainingCommandFaker WithId(Guid id)
    {
        RuleFor(e=>e.Id, id);
        return this;
    }

    public UpdateTrainingCommandFaker WithParticipants(ICollection<Employee> participants)
    {
        RuleFor(e => e.Participants, participants.Select(x=>x.Id).ToList());
        return this;    
    }
}
