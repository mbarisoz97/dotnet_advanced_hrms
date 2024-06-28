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
		var getProjectResponse = await _client.GetAsync($"{Endpoints.ProjectApi}/{Guid.NewGuid()}");

		getProjectResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	[Fact]
	public async Task Get_ExistingProject_ReturnsOkWithReadProjectDto()
	{
		var createProjectCommand = new CreateProjectCommandFaker().Generate();

		var putProjectResponse = await _client.PutAsJsonAsync(Endpoints.ProjectApi, createProjectCommand);
		putProjectResponse.EnsureSuccessStatusCode();
		var readProjectDtoFromPut = await putProjectResponse.Content.ReadFromJsonAsync<ReadProjectDto>();

		var getProjectResponse = await _client.GetAsync($"{Endpoints.ProjectApi}/{readProjectDtoFromPut!.Id}");

		getProjectResponse.StatusCode.Should().Be(HttpStatusCode.OK);

		var readProjectDtoFromGet = await getProjectResponse.Content.ReadFromJsonAsync<ReadProjectDto>();

		readProjectDtoFromGet?.Id.Should().Be(readProjectDtoFromPut.Id);
		readProjectDtoFromGet?.Should().BeEquivalentTo(createProjectCommand);
	}
}