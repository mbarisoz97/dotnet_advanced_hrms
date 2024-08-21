using Ehrms.TrainingManagement.API.Database.Models;
using System.Net;

namespace Ehrms.TrainingManagement.API.IntegrationTests.Controller.TrainingController;

public class TrainingControllerDeleteIntegrationTests : TrainingManagementBaseIntegrationTest
{
	public TrainingControllerDeleteIntegrationTests(TrainingManagementWebApplicationFactory factory) : base(factory)
	{
	}

	[Fact]
	public async Task Delete_ExistingTrainingId_ReturnsNoContent()
	{
		var traininig = await InsertRandomTraningRecord();
		var response = await client.DeleteAsync($"{Endpoints.TrainingApi}/{traininig.Id}");

		response.StatusCode.Should().Be(HttpStatusCode.NoContent);
	}

	[Fact]
	public async Task Delete_NonExistingTrainingId_ReturnsNotFound()
	{
		var response = await client.DeleteAsync($"{Endpoints.TrainingApi}/{Guid.NewGuid()}");
		response.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}
}
