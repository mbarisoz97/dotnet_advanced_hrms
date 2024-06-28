using Ehrms.ProjectManagement.API.Dtos.Project;
using Ehrms.Shared;
using System.Net;
using System.Net.Http.Headers;

namespace Ehrms.ProjectManagement.API.IntegrationTests.Controllers;

public class ProjectControllerTests : IClassFixture<ProjectManagementWebApplicationFactory>
{
    private readonly ProjectManagementWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public ProjectControllerTests(ProjectManagementWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();

        var request = new AuthenticationRequest
        {
            Username = "TestUser",
            Password = "TestPassword"
        };
        var jwt = new JwtTokenHandler().Generate(request);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt!.Token);
        _factory = factory;
    }

    [Fact]
    public async Task Put_ValidProjectDetails_ReturnsOkWithReadProjectDto()
    {
        var createProjectCommand = new CreateProjectCommandFaker().Generate();
        var putProjectResponse = await _client.PutAsJsonAsync(Endpoints.ProjectApi, createProjectCommand);

        putProjectResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var readProjectDto = await putProjectResponse.Content.ReadFromJsonAsync<ReadProjectDto>();

        readProjectDto?.Id.Should().NotBe(Guid.Empty);
        readProjectDto?.Should().BeEquivalentTo(createProjectCommand);
    }

    [Fact]
    public async Task Delete_ExistingProject_ReturnsNoContent()
    {
        var createProjectCommand = new CreateProjectCommandFaker().Generate();

        var putProjectResponse = await _client.PutAsJsonAsync(Endpoints.ProjectApi, createProjectCommand);
        putProjectResponse.EnsureSuccessStatusCode();
        var readProjectDto = await putProjectResponse.Content.ReadFromJsonAsync<ReadProjectDto>();

        var deleteProjectResponse = await _client.DeleteAsync($"{Endpoints.ProjectApi}/{readProjectDto!.Id}");

        deleteProjectResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_NonExistingProject_ReturnsNotFound()
    {
        var deleteProjectResponse = await _client.DeleteAsync($"{Endpoints.ProjectApi}/{Guid.NewGuid()}");
        
        deleteProjectResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
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

    [Fact]
    public async Task Update_NonExistingProject_ReturnsNotFound()
    {
        UpdateProjectDto updateProjectDto = new();
        var postResponse = await _client.PostAsJsonAsync(Endpoints.ProjectApi, updateProjectDto);

        postResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Post_ValidUpdateDetails_ReturnsOkWithReadProjectDto()
    {
        var createProjectCommand = new CreateProjectCommandFaker().Generate();

        var putProjectResponse = await _client.PutAsJsonAsync(Endpoints.ProjectApi, createProjectCommand);
        putProjectResponse.EnsureSuccessStatusCode();
        var readProjectDtoFromPut = await putProjectResponse.Content.ReadFromJsonAsync<ReadProjectDto>();

        var updateProjectCommand = new UpdateProjectCommandFaker()
            .WithId(readProjectDtoFromPut!.Id)
            .Generate();

        var getProjectResponse = await _client.PostAsJsonAsync($"{Endpoints.ProjectApi}", updateProjectCommand);
        var readProjectDtoFromPost = await getProjectResponse.Content.ReadFromJsonAsync<ReadProjectDto>();

        getProjectResponse.StatusCode.Should().Be(HttpStatusCode.OK);
		readProjectDtoFromPost?.Should().BeEquivalentTo(updateProjectCommand);
	}
}