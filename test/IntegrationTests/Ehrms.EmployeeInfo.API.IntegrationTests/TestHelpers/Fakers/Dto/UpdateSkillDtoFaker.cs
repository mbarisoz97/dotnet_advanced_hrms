namespace Ehrms.EmployeeInfo.API.IntegrationTests.TestHelpers.Fakers.Dto;

internal class UpdateSkillDtoFaker : Faker<UpdateSkillDto>
{
    public UpdateSkillDtoFaker()
    {
        RuleFor(e => e.Name, f => f.Name.Random.AlphaNumeric(6));
    }
}