using Ehrms.EmployeeInfo.API.IntegrationTests.TestHelpers.Configurations;

namespace Ehrms.EmployeeInfo.API.IntegrationTests.Controllers.EmployeeController;

public class EmployeeControlerPostIntegrationTests : BaseEmployeeInfoIntegrationTest
{
    public EmployeeControlerPostIntegrationTests(EmployeeInfoWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Post_ExistingEmployeeId_ReturnsOkWithUpdatedReadEmployeeDto()
    {
        var createEmployeeCommand = new CreateEmployeeCommandFaker().Generate();
        var response = await client.PutAsJsonAsync(Endpoints.EmployeeApi, createEmployeeCommand);
        response.EnsureSuccessStatusCode();
        var createEmployeeResponse = await response.Content.ReadFromJsonAsync<ReadEmployeeDto>();

        var updateEmployeeCommand = new UpdateEmployeeCommandFaker()
            .WithId(createEmployeeResponse!.Id)
            .Generate();

        response = await client.PostAsJsonAsync(Endpoints.EmployeeApi, updateEmployeeCommand);
        response.EnsureSuccessStatusCode();
        var updateEmployeeResponse = await response.Content.ReadFromJsonAsync<ReadEmployeeDto>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        updateEmployeeResponse.Should().BeEquivalentTo(updateEmployeeCommand);
    }

    [Fact]
    public async Task Post_NonExistingEmployeeId_ReturnsNotFound()
    {
        var command = new UpdateEmployeeCommandFaker().Generate();
        var response = await client.PostAsJsonAsync(Endpoints.EmployeeApi, command);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Post_EmptyShortFirstName_ReturnsBadRequest()
    {
        var command = new UpdateEmployeeCommandFaker().Generate();
        command.FirstName = "s";
        var response = await client.PostAsJsonAsync(Endpoints.EmployeeApi, command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Post_EmptyOrShortLastName_ReturnsBadRequest()
    {
        var command = new UpdateEmployeeCommandFaker().Generate();
        command.LastName = "s";
        var response = await client.PostAsJsonAsync(Endpoints.EmployeeApi, command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Post_EmptyId_RetursBadRequest()
    {
        var command = new UpdateEmployeeCommandFaker().Generate();
        command.Id = Guid.Empty;
        var response = await client.PostAsJsonAsync(Endpoints.EmployeeApi, command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}