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
        CreateEmployeeDto createEmployeeDto = new CreateEmployeeDtoFaker().Generate();

        var response = await _client.PutAsJsonAsync(Endpoints.EmployeeApi, createEmployeeDto);
        var createEmployeeResponse = await response.Content.ReadFromJsonAsync<ReadEmployeeDto>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        createEmployeeResponse?.Id.Should().NotBe(Guid.Empty);
        createEmployeeResponse?.FirstName.Should().Be(createEmployeeDto.FirstName);
        createEmployeeResponse?.LastName.Should().Be(createEmployeeDto.LastName);
        createEmployeeResponse?.Qualification.Should().Be(createEmployeeDto.Qualification);
        createEmployeeResponse?.DateOfBirth.Should().Be(createEmployeeDto.DateOfBirth);
    }

    [Fact]
    public async Task Get_ExistingEmployeeId_ReturnsOkWithEmployeeReadDto()
    {
        CreateEmployeeDto createEmployeeDto = new CreateEmployeeDtoFaker().Generate();

        var response = await _client.PutAsJsonAsync(Endpoints.EmployeeApi, createEmployeeDto);
        response.EnsureSuccessStatusCode();

        var createEmployeeResponse = await response.Content.ReadFromJsonAsync<ReadEmployeeDto>();
        response = await _client.GetAsync($"{Endpoints.EmployeeApi}/{createEmployeeResponse?.Id}");
        var readEmployeeResponse = await response.Content.ReadFromJsonAsync<ReadEmployeeDto>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        readEmployeeResponse?.Id.Should().NotBe(Guid.Empty);
        readEmployeeResponse?.FirstName.Should().Be(createEmployeeDto.FirstName);
        readEmployeeResponse?.LastName.Should().Be(createEmployeeDto.LastName);
        readEmployeeResponse?.Qualification.Should().Be(createEmployeeDto.Qualification);
        readEmployeeResponse?.DateOfBirth.Should().Be(createEmployeeDto.DateOfBirth);
    }

    [Fact]
    public async Task Delete_ExistingEmployeeId_RemovesEmployee()
    {
        CreateEmployeeDto createEmployeeDto = new CreateEmployeeDtoFaker().Generate();

        var response = await _client.PutAsJsonAsync(Endpoints.EmployeeApi, createEmployeeDto);
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
        CreateEmployeeDto createEmployeeDto = new CreateEmployeeDtoFaker().Generate();
        var response = await _client.PutAsJsonAsync(Endpoints.EmployeeApi, createEmployeeDto);
        response.EnsureSuccessStatusCode();
        var createEmployeeResponse = await response.Content.ReadFromJsonAsync<ReadEmployeeDto>();

        UpdateEmployeeDto updateEmployeeDto = new UpdateEmployeeDtoFaker().Generate();
        updateEmployeeDto.Id = createEmployeeResponse!.Id;

        response = await _client.PostAsJsonAsync(Endpoints.EmployeeApi, updateEmployeeDto);
        response.EnsureSuccessStatusCode();
        var updateEmployeeResponse = await response.Content.ReadFromJsonAsync<ReadEmployeeDto>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        updateEmployeeResponse?.Id.Should().Be(updateEmployeeDto.Id);
        updateEmployeeResponse?.FirstName.Should().Be(updateEmployeeDto.FirstName);
        updateEmployeeResponse?.LastName.Should().Be(updateEmployeeDto.LastName);
        updateEmployeeResponse?.Qualification.Should().Be(updateEmployeeDto.Qualification);
        updateEmployeeResponse?.DateOfBirth.Should().Be(updateEmployeeDto.DateOfBirth);
    }
}