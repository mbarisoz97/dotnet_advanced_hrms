namespace Ehrms.EmployeeInfo.API.IntegrationTests.Controllers;

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