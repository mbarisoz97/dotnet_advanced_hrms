namespace Ehrms.TrainingManagement.API.IntegrationTests.TestHelpers.Fakers.Dto;

internal class UpdateTrainingDtoFaker : Faker<UpdateTrainingDto>
{
    public UpdateTrainingDtoFaker()
    {
        RuleFor(e => e.Name, f => f.Random.Word());
        RuleFor(e => e.PlannedAt, f => f.Date.Future());
        RuleFor(e => e.Description, f => f.Random.Words());
    }
    
    public UpdateTrainingDtoFaker WithId(Guid id)
    {
        RuleFor(e => e.Id, id);
        return this;
    }
}