using Ehrms.EmployeeInfo.API.IntegrationTests.TestHelpers.Configurations;

namespace Ehrms.EmployeeInfo.API.IntegrationTests.Controllers.EmployeeController;

public class EmployeeControlerPutIntegrationTests : BaseEmployeeInfoIntegrationTest
{
    public EmployeeControlerPutIntegrationTests(EmployeeInfoWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Put_ValidEmployeeDetails_ReturnsOkWithEmployeeDto()
    {
        var command = new CreateEmployeeCommandFaker().Generate();

        var response = await client.PutAsJsonAsync(Endpoints.EmployeeApi, command);
        var createEmployeeResponse = await response.Content.ReadFromJsonAsync<ReadEmployeeDto>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        createEmployeeResponse?.Id.Should().NotBe(Guid.Empty);
        createEmployeeResponse?.Should().BeEquivalentTo(command);
    }

    [Fact]
    public async Task Put_ShortFirstName_ReturnsBadRequest()
    {
        var command = new CreateEmployeeCommandFaker().Generate();
        command.FirstName = "s";

        var response = await client.PutAsJsonAsync(Endpoints.EmployeeApi, command);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Put_ShortLastNameName_ReturnsBadRequest()
    {
        var command = new CreateEmployeeCommandFaker().Generate();
        command.LastName = "t";

        var response = await client.PutAsJsonAsync(Endpoints.EmployeeApi, command);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
