using System.Net;

namespace Ehrms.ProjectManagement.API.IntegrationTests.Controllers.ProjectController;

public class ProjectControllerPutIntegrationTests : ProjectManagementApiBaseIntegrationTests
{
	public ProjectControllerPutIntegrationTests(ProjectManagementWebApplicationFactory factory) : base(factory)
	{
	}

	[Fact]
	public async Task Put_ValidProjectDetails_ReturnsOkWithReadProjectDto()
	{
		var createProjectCommand = new CreateProjectCommandFaker().Generate();
		var putProjectResponse = await client.PutAsJsonAsync(Endpoints.ProjectApi, createProjectCommand);

		putProjectResponse.StatusCode.Should().Be(HttpStatusCode.OK);
		var readProjectDto = await putProjectResponse.Content.ReadFromJsonAsync<ReadProjectDto>();

		readProjectDto?.Id.Should().NotBe(Guid.Empty);
		readProjectDto?.Should().BeEquivalentTo(createProjectCommand);
	}

	[Fact]
	public async Task Put_ShortProjectName_ReturnsBadRequest()
	{
		var command = new CreateProjectCommandFaker().Generate();
		command.Name = "s";
		var putProjectResponse = await client.PutAsJsonAsync(Endpoints.ProjectApi, command);

		putProjectResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task Put_EmptyProjectDecription_ReturnsBadRequest()
	{
		var command = new CreateProjectCommandFaker().Generate();
		command.Description = "";
		var putProjectResponse = await client.PutAsJsonAsync(Endpoints.ProjectApi, command);

		putProjectResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}
}