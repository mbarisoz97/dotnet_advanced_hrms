using Ehrms.Shared;
using System.Net.Http.Headers;

namespace Ehrms.EmployeeInfo.API.IntegrationTests.Controllers;

public class EmployeeControllerTests : IClassFixture<EmployeeInfoWebApplicationFactory>
{
    private readonly EmployeeInfoWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public EmployeeControllerTests(EmployeeInfoWebApplicationFactory factory)
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
    public async Task Put_ValidEmployeeDetails_ReturnsOkWithEmployeeDto()
    {
        var command = new CreateEmployeeCommandFaker().Generate();

        var response = await _client.PutAsJsonAsync(Endpoints.EmployeeApi, command);
        var createEmployeeResponse = await response.Content.ReadFromJsonAsync<ReadEmployeeDto>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        createEmployeeResponse?.Id.Should().NotBe(Guid.Empty);
        createEmployeeResponse?.Should().BeEquivalentTo(command);
    }

    [Fact]
    public async Task Get_ExistingEmployeeId_ReturnsOkWithEmployeeReadDto()
    {
        var createEmployeeCommand = new CreateEmployeeCommandFaker().Generate();

        var response = await _client.PutAsJsonAsync(Endpoints.EmployeeApi, createEmployeeCommand);
        response.EnsureSuccessStatusCode();

        var createEmployeeResponse = await response.Content.ReadFromJsonAsync<ReadEmployeeDto>();
        response = await _client.GetAsync($"{Endpoints.EmployeeApi}/{createEmployeeResponse?.Id}");
        var readEmployeeResponse = await response.Content.ReadFromJsonAsync<ReadEmployeeDto>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        readEmployeeResponse?.Id.Should().NotBe(Guid.Empty);
        readEmployeeResponse.Should().BeEquivalentTo(createEmployeeCommand);
    }

    [Fact]
    public async Task Delete_ExistingEmployeeId_RemovesEmployee()
    {
        var createEmployeeCommand = new CreateEmployeeCommandFaker().Generate();

        var response = await _client.PutAsJsonAsync(Endpoints.EmployeeApi, createEmployeeCommand);
        response.EnsureSuccessStatusCode();

        var createEmployeeResponse = await response.Content.ReadFromJsonAsync<ReadEmployeeDto>();
        response = await _client.DeleteAsync($"{Endpoints.EmployeeApi}/{createEmployeeResponse?.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Delete_NonExistingEmployeeId_ReturnsNotFound()
    {
        var response = await _client.DeleteAsync($"{Endpoints.EmployeeApi}/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Post_ExistingEmployeeId_ReturnsOkWithUpdatedReadEmployeeDto()
    {
        var createEmployeeCommand = new CreateEmployeeCommandFaker().Generate();
        var response = await _client.PutAsJsonAsync(Endpoints.EmployeeApi, createEmployeeCommand);
        response.EnsureSuccessStatusCode();
        var createEmployeeResponse = await response.Content.ReadFromJsonAsync<ReadEmployeeDto>();

        var updateEmployeeCommand = new UpdateEmployeeCommandFaker()
            .WithId(createEmployeeResponse!.Id)
            .Generate();

        response = await _client.PostAsJsonAsync(Endpoints.EmployeeApi, updateEmployeeCommand);
        response.EnsureSuccessStatusCode();
        var updateEmployeeResponse = await response.Content.ReadFromJsonAsync<ReadEmployeeDto>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        updateEmployeeResponse.Should().BeEquivalentTo(updateEmployeeCommand);
    }
}