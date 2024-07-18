using Ehrms.ProjectManagement.API.Database.Models;
using Ehrms.ProjectManagement.API.Handlers.Project.Queries;
using Ehrms.ProjectManagemet.TestHelpers.Fakers.Model;
using System.Net;

namespace Ehrms.ProjectManagement.API.IntegrationTests.Controllers.ProjectController;

public class ProjectControllerGetIntegrationTests : ProjectManagementApiBaseIntegrationTests
{
	public ProjectControllerGetIntegrationTests(ProjectManagementWebApplicationFactory factory) : base(factory)
	{
	}

	[Fact]
	public async Task Get_NonExistingProject_ReturnsNotFound()
	{
		var getProjectResponse = await client.GetAsync($"{Endpoints.ProjectApi}/{Guid.NewGuid()}");
		getProjectResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	[Fact]
	public async Task Get_ExistingProject_ReturnsOkWithReadProjectDto()
	{
		var createProjectCommand = new CreateProjectCommandFaker().Generate();

		var putProjectResponse = await client.PutAsJsonAsync(Endpoints.ProjectApi, createProjectCommand);
		putProjectResponse.EnsureSuccessStatusCode();
		var readProjectDtoFromPut = await putProjectResponse.Content.ReadFromJsonAsync<ReadProjectDto>();

		var getProjectResponse = await client.GetAsync($"{Endpoints.ProjectApi}/{readProjectDtoFromPut!.Id}");

		getProjectResponse.StatusCode.Should().Be(HttpStatusCode.OK);

		var readProjectDtoFromGet = await getProjectResponse.Content.ReadFromJsonAsync<ReadProjectDto>();

		readProjectDtoFromGet?.Id.Should().Be(readProjectDtoFromPut.Id);
		readProjectDtoFromGet?.Should().BeEquivalentTo(createProjectCommand);
	}

	[Fact]
	public async Task Get_ExistingProjectWithRequiredSkills_ReturnsOkWithReadProjectDto()
	{
		var skills = new SkillFaker().Generate(4);
		await dbContext.AddRangeAsync(skills);

		var employments = Array.Empty<Employment>();
		var project = new ProjectFaker()
			.WithRequiredSkills(skills)
			.WithEmployments(employments)
			.Generate();
		await dbContext.AddAsync(project);
		await dbContext.SaveChangesAsync();

		var getProjectResponse = await client.GetAsync($"{Endpoints.ProjectApi}/{project!.Id}");

		getProjectResponse.StatusCode.Should().Be(HttpStatusCode.OK);
		var readProjectDtoFromGet = await getProjectResponse.Content.ReadFromJsonAsync<ReadProjectDto>();

		readProjectDtoFromGet.Should().BeEquivalentTo(project, options =>
			options.Excluding(o => o.RequiredProjectSkills)
			.Excluding(o => o.Employments));
		
		readProjectDtoFromGet?.RequiredSkills.Should().HaveCount(skills.Count);
		readProjectDtoFromGet?.Employees.Should().HaveCount(employments.Length);
	}
}