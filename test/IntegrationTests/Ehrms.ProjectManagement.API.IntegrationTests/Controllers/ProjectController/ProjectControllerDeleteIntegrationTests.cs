using System.Net;

namespace Ehrms.ProjectManagement.API.IntegrationTests.Controllers.ProjectController;

public class ProjectControllerDeleteIntegrationTests : ProjectManagementApiBaseIntegrationTests
{
    public ProjectControllerDeleteIntegrationTests(ProjectManagementWebApplicationFactory factory) : base(factory)
    {
    }

	[Fact]
	public async Task Delete_ExistingProject_ReturnsNoContent()
	{
		var createProjectCommand = new CreateProjectCommandFaker().Generate();

		var putProjectResponse = await client.PutAsJsonAsync(Endpoints.ProjectApi, createProjectCommand);
		putProjectResponse.EnsureSuccessStatusCode();
		var readProjectDto = await putProjectResponse.Content.ReadFromJsonAsync<ReadProjectDto>();

		var deleteProjectResponse = await client.DeleteAsync($"{Endpoints.ProjectApi}/{readProjectDto!.Id}");

		deleteProjectResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
	}

	[Fact]
	public async Task Delete_NonExistingProject_ReturnsNotFound()
	{
		var deleteProjectResponse = await client.DeleteAsync($"{Endpoints.ProjectApi}/{Guid.NewGuid()}");
		deleteProjectResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}
}
