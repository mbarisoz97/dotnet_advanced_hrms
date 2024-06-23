using System.Net;

namespace Ehrms.ProjectManagement.API.IntegrationTests.Controllers;

internal static class Endpoints
{
    internal const string ProjectApi = "/api/Project";
}

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
}
