using Ehrms.Shared;
using System.Net.Http.Headers;

namespace Ehrms.EmployeeInfo.API.IntegrationTests.Controllers;

public class SkillControllerTests : IClassFixture<EmployeeInfoWebApplicationFactory>
{
    private readonly EmployeeInfoWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public SkillControllerTests(EmployeeInfoWebApplicationFactory factory)
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
    public async Task Put_ValidSkillName_ReturnsOkWithReadSkillDto()
    {
        CreateSkillDto createSkillDto = new CreateSkillDtoFaker().Generate();

        var response = await _client.PutAsJsonAsync(Endpoints.EmployeeSkillsApi, createSkillDto);
        var createSkillResponse = await response.Content.ReadFromJsonAsync<ReadSkillDto>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        createSkillResponse?.Id.Should().NotBe(Guid.Empty);
        createSkillResponse?.Name.Should().Be(createSkillDto.Name);
    }

    [Fact]
    public async Task Get_ExistingSkillId_ReturnsOkWithReadSkillDto()
    {
        CreateSkillDto createSkillDto = new CreateSkillDtoFaker().Generate();

        var response = await _client.PutAsJsonAsync(Endpoints.EmployeeSkillsApi, createSkillDto);
        response.EnsureSuccessStatusCode();
        var createSkillResponse = await response.Content.ReadFromJsonAsync<ReadSkillDto>();

        response = await _client.GetAsync($"{Endpoints.EmployeeSkillsApi}/{createSkillResponse?.Id}");
        var readSkillDto = await response.Content.ReadFromJsonAsync<ReadSkillDto>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        readSkillDto?.Id.Should().Be(createSkillResponse!.Id);
        readSkillDto?.Name.Should().Be(createSkillResponse!.Name);
    }

    [Fact]
    public async Task Get_NonExistingSkillId_ReturnsNotFound()
    {
        var response = await _client.GetAsync($"{Endpoints.EmployeeSkillsApi}/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_ExistingSkillId_ReturnsNoContent()
    {
        CreateSkillDto createSkillDto = new CreateSkillDtoFaker().Generate();

        var response = await _client.PutAsJsonAsync(Endpoints.EmployeeSkillsApi, createSkillDto);
        response.EnsureSuccessStatusCode();
        var createSkillResponse = await response.Content.ReadFromJsonAsync<ReadEmployeeDto>();
        response = await _client.DeleteAsync($"{Endpoints.EmployeeSkillsApi}/{createSkillResponse?.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_NonExistingSkillId_ReturnsNotFound()
    {
        var response = await _client.DeleteAsync($"{Endpoints.EmployeeSkillsApi}/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Post_NonExistingSkillId_ReturnsNotFound()
    {
        var updateSkillDto = new UpdateSkillDtoFaker().Generate();
        var response = await _client.PostAsJsonAsync(Endpoints.EmployeeSkillsApi, updateSkillDto);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}