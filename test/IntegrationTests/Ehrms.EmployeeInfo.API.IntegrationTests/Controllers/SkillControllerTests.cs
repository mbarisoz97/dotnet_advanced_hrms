using Ehrms.EmployeeInfo.API.Dtos.Skill;

namespace Ehrms.EmployeeInfo.API.IntegrationTests.Controllers;

public class SkillControllerTests
{
    [Fact]
    public async Task Put_ValidSkillName_ReturnsOkWithReadSkillDto()
    {
        EmployeeInfoWebApplicationFactory application = new();
        CreateSkillDto createSkillDto = new CreateSkillDtoFaker().Generate();

        var client = application.CreateClient();
        var response = await client.PutAsJsonAsync(Endpoints.EmployeeSkillsApi, createSkillDto);
        var createSkillResponse = await response.Content.ReadFromJsonAsync<ReadSkillDto>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        createSkillResponse?.Id.Should().NotBe(Guid.Empty);
        createSkillResponse?.Name.Should().Be(createSkillDto.Name);
    }

    [Fact]
    public async Task Get_ExistingSkillId_ReturnsOkWithReadSkillDto()
    {
        EmployeeInfoWebApplicationFactory application = new();
        CreateSkillDto createSkillDto = new CreateSkillDtoFaker().Generate();

        var client = application.CreateClient();
        var response = await client.PutAsJsonAsync(Endpoints.EmployeeSkillsApi, createSkillDto);
        response.EnsureSuccessStatusCode();
        var createSkillResponse = await response.Content.ReadFromJsonAsync<ReadSkillDto>();

        response = await client.GetAsync($"{Endpoints.EmployeeSkillsApi}/{createSkillResponse?.Id}");
        var readSkillDto = await response.Content.ReadFromJsonAsync<ReadSkillDto>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        readSkillDto?.Id.Should().Be(createSkillResponse!.Id);
        readSkillDto?.Name.Should().Be(createSkillResponse!.Name);
    }

    [Fact]
    public async Task Get_NonExistingSkillId_ReturnsNotFound()
    {
        EmployeeInfoWebApplicationFactory application = new();
        var client = application.CreateClient();
        var response = await client.GetAsync($"{Endpoints.EmployeeSkillsApi}/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_ExistingSkillId_ReturnsNoContent()
    {
        EmployeeInfoWebApplicationFactory application = new();
        CreateSkillDto createSkillDto = new CreateSkillDtoFaker().Generate();

        var client = application.CreateClient();
        var response = await client.PutAsJsonAsync(Endpoints.EmployeeSkillsApi, createSkillDto);
        response.EnsureSuccessStatusCode();
        var createSkillResponse = await response.Content.ReadFromJsonAsync<ReadEmployeeDto>();

        response = await client.DeleteAsync($"{Endpoints.EmployeeSkillsApi}/{createSkillResponse?.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_NonExistingSkillId_ReturnsNotFound()
    {
        EmployeeInfoWebApplicationFactory application = new();
        var client = application.CreateClient();
        var response = await client.DeleteAsync($"{Endpoints.EmployeeSkillsApi}/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Post_NonExistingSkillId_ReturnsNotFound()
    {
        EmployeeInfoWebApplicationFactory application = new();
        var client = application.CreateClient();
        var updateSkillDto = new UpdateSkillDtoFaker().Generate();

        var response = await client.PostAsJsonAsync(Endpoints.EmployeeSkillsApi, updateSkillDto);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}