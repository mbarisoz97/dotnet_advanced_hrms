using System.Net;
using System.Net.Http.Json;

namespace Ehrms.TrainingManagement.API.IntegrationTests.Controller.TrainingController;

public class TrainingControllerGetIntegrationTests : TrainingManagementBaseIntegrationTest
{
	public TrainingControllerGetIntegrationTests(TrainingManagementWebApplicationFactory factory) : base(factory)
	{
	}

	[Fact]
	public async Task Get_ExistingTrainingId_ReturnsOkWithReadTrainingDto()
	{
		var training = await InsertRandomTraningRecord();

		var response = await client.GetAsync($"{Endpoints.TrainingApi}/{training.Id}");
		var readTrainingResponse = await response.Content.ReadFromJsonAsync<ReadTrainingDto>();

		response.StatusCode.Should().Be(HttpStatusCode.OK);
		readTrainingResponse?.Should().BeEquivalentTo(training);
	}

	[Fact]
	public async Task Get_ReturnsOkWithCollectionOfReadTrainingDtos()
	{
		await InsertRandomTraningRecord();

		var response = await client.GetAsync($"{Endpoints.TrainingApi}");
		var readTrainingResponse = await response.Content.ReadFromJsonAsync<List<ReadTrainingDto>>();

		response.StatusCode.Should().Be(HttpStatusCode.OK);
		readTrainingResponse.Should().NotBeEmpty();
	}
}