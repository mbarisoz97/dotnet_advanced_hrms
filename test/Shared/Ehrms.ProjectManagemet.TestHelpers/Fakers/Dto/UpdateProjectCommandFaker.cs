namespace Ehrms.ProjectManagement.API.TestHelpers.Faker.Dto;
internal class UpdateProjectCommandFaker : Faker<UpdateProjectCommand>
{
	public UpdateProjectCommandFaker()
	{
		RuleFor(x => x.Name, f => f.Name.Random.AlphaNumeric(10));
		RuleFor(x => x.Description, f => f.Name.Random.AlphaNumeric(10));
	}

	public UpdateProjectCommandFaker WithId(Guid id)
	{
		RuleFor(x => x.Id, id);
		return this;
	}

}