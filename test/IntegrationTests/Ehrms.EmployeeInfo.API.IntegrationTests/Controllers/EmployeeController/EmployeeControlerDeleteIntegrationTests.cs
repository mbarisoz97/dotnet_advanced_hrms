namespace Ehrms.EmployeeInfo.API.IntegrationTests.Controllers;

public class EmployeeControlerDeleteIntegrationTests : BaseEmployeeInfoIntegrationTest
{
	public EmployeeControlerDeleteIntegrationTests(EmployeeInfoWebApplicationFactory factory) : base(factory)
	{
	}

	[Fact]
	public async Task Delete_ExistingEmployeeId_RemovesEmployee()
	{
		var createEmployeeCommand = new CreateEmployeeCommandFaker().Generate();

		var response = await client.PutAsJsonAsync(Endpoints.EmployeeApi, createEmployeeCommand);
		response.EnsureSuccessStatusCode();

		var createEmployeeResponse = await response.Content.ReadFromJsonAsync<ReadEmployeeDto>();
		response = await client.DeleteAsync($"{Endpoints.EmployeeApi}/{createEmployeeResponse?.Id}");

		response.StatusCode.Should().Be(HttpStatusCode.OK);
	}

	[Fact]
	public async Task Delete_NonExistingEmployeeId_ReturnsNotFound()
	{
		var response = await client.DeleteAsync($"{Endpoints.EmployeeApi}/{Guid.NewGuid()}");

		response.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

}
