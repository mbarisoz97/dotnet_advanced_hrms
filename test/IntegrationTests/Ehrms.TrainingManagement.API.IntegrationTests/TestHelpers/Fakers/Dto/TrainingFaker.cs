using Ehrms.TrainingManagement.API.Models;

namespace Ehrms.TrainingManagement.API.IntegrationTests.TestHelpers.Fakers.Dto;

internal class TrainingFaker : Faker<Training>
{
    public TrainingFaker()
    {
        RuleFor(e => e.Name, f => f.Random.Word());
        RuleFor(e => e.PlannedAt, f => f.Date.Future());
        RuleFor(e => e.Description, f => f.Random.Words());
    }
}