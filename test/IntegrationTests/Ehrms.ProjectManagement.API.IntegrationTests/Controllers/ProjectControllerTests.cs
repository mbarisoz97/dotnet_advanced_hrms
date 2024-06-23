using System.Net;

namespace Ehrms.ProjectManagement.API.IntegrationTests.Controllers;

public class ProjectControllerTests
{
    [Fact]
    public async Task Put_ValidProjectDetails_ReturnsOkWithReadProjectDto()
    {
        var application = new ProjectManagementWebApplicationFactory();
        CreateProjectDto createProjectDto = new CreateProjectDtoFaker().Generate();

        var client = application.CreateClient();
        var putProjectResponse = await client.PutAsJsonAsync(Endpoints.ProjectApi, createProjectDto);

        putProjectResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var readProjectDto = await putProjectResponse.Content.ReadFromJsonAsync<ReadProjectDto>();

        readProjectDto?.Id.Should().NotBe(Guid.Empty);
        readProjectDto?.Name.Should().Be(createProjectDto.Name);
        readProjectDto?.Description.Should().Be(createProjectDto.Description);
    }

    [Fact]
    public async Task Delete_ExistingProject_ReturnsNoContent()
    {
        var application = new ProjectManagementWebApplicationFactory();
        CreateProjectDto createProjectDto = new CreateProjectDtoFaker().Generate();

        var client = application.CreateClient();
        var putProjectResponse = await client.PutAsJsonAsync(Endpoints.ProjectApi, createProjectDto);
        putProjectResponse.EnsureSuccessStatusCode();
        var readProjectDto = await putProjectResponse.Content.ReadFromJsonAsync<ReadProjectDto>();

        var deleteProjectResponse = await client.DeleteAsync($"{Endpoints.ProjectApi}/{readProjectDto!.Id}");

        deleteProjectResponse.StatusCode.Should()
            .Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_NonExistingProject_ReturnsNotFound()
    {
        var application = new ProjectManagementWebApplicationFactory();

        var client = application.CreateClient();
        var deleteProjectResponse = await client.DeleteAsync($"{Endpoints.ProjectApi}/{Guid.NewGuid()}");

        deleteProjectResponse.StatusCode.Should()
             .Be(HttpStatusCode.NotFound);
    }
}
