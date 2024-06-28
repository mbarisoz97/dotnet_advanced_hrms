namespace Ehrms.EmployeeInfo.API.IntegrationTests.Controllers;

public class EmployeeControlerGetIntegrationTests : BaseEmployeeInfoIntegrationTest
{
	public EmployeeControlerGetIntegrationTests(EmployeeInfoWebApplicationFactory factory) : base(factory)
	{
	}

	[Fact]
	public async Task Get_ExistingEmployeeId_ReturnsOkWithEmployeeReadDto()
	{
		var createEmployeeCommand = new CreateEmployeeCommandFaker().Generate();

		var response = await client.PutAsJsonAsync(Endpoints.EmployeeApi, createEmployeeCommand);
		response.EnsureSuccessStatusCode();

		var createEmployeeResponse = await response.Content.ReadFromJsonAsync<ReadEmployeeDto>();
		response = await client.GetAsync($"{Endpoints.EmployeeApi}/{createEmployeeResponse?.Id}");
		var readEmployeeResponse = await response.Content.ReadFromJsonAsync<ReadEmployeeDto>();

		response.StatusCode.Should().Be(HttpStatusCode.OK);
		readEmployeeResponse?.Id.Should().NotBe(Guid.Empty);
		readEmployeeResponse.Should().BeEquivalentTo(createEmployeeCommand);
	}
}
