using Ehrms.ProjectManagement.API.Database.Models;

namespace Ehrms.ProjectManagement.API.UnitTests.TestHelpers.Faker.Command;

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
	public UpdateProjectCommandFaker WithRequiredSkills(ICollection<Skill> skills)
	{
		var idCollection = skills.Select(x => x.Id).ToList();
		RuleFor(x => x.RequiredSkills, idCollection);
		return this;
	}
}