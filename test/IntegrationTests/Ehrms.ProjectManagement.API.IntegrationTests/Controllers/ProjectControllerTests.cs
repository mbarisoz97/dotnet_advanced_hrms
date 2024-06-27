using Ehrms.Shared;
using System.Net;
using System.Net.Http.Headers;

namespace Ehrms.ProjectManagement.API.IntegrationTests.Controllers;

public class ProjectControllerTests : IAsyncLifetime
{
    private readonly ProjectManagementWebApplicationFactory _application;
    private readonly HttpClient _client;

    public ProjectControllerTests()
    {
        _application = new();
        _client = _application.CreateClient();

        var request = new AuthenticationRequest
        {
            Username = "TestUser",
            Password = "TestPassword"
        };
        var jwt = new JwtTokenHandler().Generate(request);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt!.Token);
    }

    [Fact]
    public async Task Put_ValidProjectDetails_ReturnsOkWithReadProjectDto()
    {
        CreateProjectDto createProjectDto = new CreateProjectDtoFaker().Generate();
        var putProjectResponse = await _client.PutAsJsonAsync(Endpoints.ProjectApi, createProjectDto);

        putProjectResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var readProjectDto = await putProjectResponse.Content.ReadFromJsonAsync<ReadProjectDto>();

        readProjectDto?.Id.Should().NotBe(Guid.Empty);
        readProjectDto?.Name.Should().Be(createProjectDto.Name);
        readProjectDto?.Description.Should().Be(createProjectDto.Description);
    }

    [Fact]
    public async Task Delete_ExistingProject_ReturnsNoContent()
    {
        CreateProjectDto createProjectDto = new CreateProjectDtoFaker().Generate();

        var putProjectResponse = await _client.PutAsJsonAsync(Endpoints.ProjectApi, createProjectDto);
        putProjectResponse.EnsureSuccessStatusCode();
        var readProjectDto = await putProjectResponse.Content.ReadFromJsonAsync<ReadProjectDto>();

        var deleteProjectResponse = await _client.DeleteAsync($"{Endpoints.ProjectApi}/{readProjectDto!.Id}");

        deleteProjectResponse.StatusCode.Should()
            .Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_NonExistingProject_ReturnsNotFound()
    {
        var deleteProjectResponse = await _client.DeleteAsync($"{Endpoints.ProjectApi}/{Guid.NewGuid()}");

        deleteProjectResponse.StatusCode.Should()
             .Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Get_NonExistingProject_ReturnsNotFound()
    {
        var getProjectResponse = await _client.GetAsync($"{Endpoints.ProjectApi}/{Guid.NewGuid()}");

        getProjectResponse.StatusCode.Should()
            .Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Get_ExistingProject_ReturnsOkWithReadProjectDto()
    {
        CreateProjectDto createProjectDto = new CreateProjectDtoFaker().Generate();

        var putProjectResponse = await _client.PutAsJsonAsync(Endpoints.ProjectApi, createProjectDto);
        putProjectResponse.EnsureSuccessStatusCode();
        var readProjectDtoFromPut = await putProjectResponse.Content.ReadFromJsonAsync<ReadProjectDto>();

        var getProjectResponse = await _client.GetAsync($"{Endpoints.ProjectApi}/{readProjectDtoFromPut!.Id}");

        getProjectResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var readProjectDtoFromGet = await getProjectResponse.Content.ReadFromJsonAsync<ReadProjectDto>();

        readProjectDtoFromGet?.Id.Should().Be(readProjectDtoFromPut.Id);
        readProjectDtoFromGet?.Name.Should().Be(readProjectDtoFromPut.Name);
        readProjectDtoFromGet?.Description.Should().Be(readProjectDtoFromPut.Description);
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
        CreateProjectDto createProjectDto = new CreateProjectDtoFaker().Generate();

        var putProjectResponse = await _client.PutAsJsonAsync(Endpoints.ProjectApi, createProjectDto);
        putProjectResponse.EnsureSuccessStatusCode();
        var readProjectDtoFromPut = await putProjectResponse.Content.ReadFromJsonAsync<ReadProjectDto>();

        UpdateProjectDto updateProjectDto = new UpdateProjectDtoFaker().Generate();
        updateProjectDto.Id = readProjectDtoFromPut!.Id;

        var getProjectResponse = await _client.PostAsJsonAsync($"{Endpoints.ProjectApi}", updateProjectDto);
        var readProjectDtoFromPost = await getProjectResponse.Content.ReadFromJsonAsync<ReadProjectDto>();

        getProjectResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        readProjectDtoFromPost?.Name.Should().Be(updateProjectDto.Name);
        readProjectDtoFromPost?.Description.Should().Be(updateProjectDto.Description);
    }

    public async Task InitializeAsync()
    {
        await _application.Start();
    }

    public async Task DisposeAsync()
    {
        await _application.Stop();
    }
}