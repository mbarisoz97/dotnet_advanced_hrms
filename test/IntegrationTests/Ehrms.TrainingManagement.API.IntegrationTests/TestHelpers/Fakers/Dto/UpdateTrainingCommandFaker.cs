using Ehrms.TrainingManagement.API.Handlers.Training.Commands;

namespace Ehrms.TrainingManagement.API.IntegrationTests.TestHelpers.Fakers.Dto;

internal class UpdateTrainingCommandFaker : Faker<UpdateTrainingCommand>
{
    public UpdateTrainingCommandFaker()
    {
        RuleFor(e => e.Name, f => f.Random.Word());
        RuleFor(e => e.PlannedAt, f => f.Date.Future());
        RuleFor(e => e.Description, f => f.Random.Words());
    }
    
    public UpdateTrainingCommandFaker WithId(Guid id)
    {
        RuleFor(e => e.Id, id);
        return this;
    }
}