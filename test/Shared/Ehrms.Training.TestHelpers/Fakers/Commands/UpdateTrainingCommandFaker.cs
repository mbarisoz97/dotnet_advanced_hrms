namespace Ehrms.Training.TestHelpers.Fakers.Commands;

public sealed class UpdateTrainingCommandFaker : Faker<UpdateTrainingCommand>
{
    public UpdateTrainingCommandFaker()
    {
        RuleFor(e => e.Name, f => f.Random.Word());
        RuleFor(e => e.PlannedAt, f => f.Date.Future());
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
