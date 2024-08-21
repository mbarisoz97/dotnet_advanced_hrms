using Ehrms.TrainingManagement.API.Handlers.Training.Commands;

namespace Ehrms.TrainingManagement.API.IntegrationTests.TestHelpers.Fakers.Dto;

public sealed class UpdateTrainingCommandFaker : Faker<UpdateTrainingCommand>
{
    public UpdateTrainingCommandFaker()
    {
        RuleFor(e => e.Name, f => f.Random.Word());
        RuleFor(e => e.StartsAt, f => DateTime.Today.AddDays(1));
        RuleFor(e => e.EndsAt, f => DateTime.Today.AddDays(1).AddHours(1));
        RuleFor(e => e.Description, f => f.Random.Words());
    }
    
    public UpdateTrainingCommandFaker WithId(Guid id)
    {
        RuleFor(e => e.Id, id);
        return this;
    }
}