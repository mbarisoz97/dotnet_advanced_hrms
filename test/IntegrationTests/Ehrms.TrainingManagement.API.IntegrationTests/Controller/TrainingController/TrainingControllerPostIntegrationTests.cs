using Ehrms.TrainingManagement.API.Handlers.Training.Commands;
using Ehrms.TrainingManagement.API.Models;
using System.Net;
using System.Net.Http.Json;

namespace Ehrms.TrainingManagement.API.IntegrationTests.Controller.TrainingController;

public class TrainingControllerPostIntegrationTests : TrainingManagementBaseIntegrationTest
{
	public TrainingControllerPostIntegrationTests(TrainingManagementWebApplicationFactory factory) : base(factory)
	{
	}

	[Fact]
	public async Task Post_ExistingTrainingIdWithValidDetails_ReturnsOkWithReadTrainingDto()
	{
		Training traininig = await InsertRandomTraningRecord();

		var updateTrainingCommand = new UpdateTrainingCommandFaker()
			.WithId(traininig.Id)
			.Generate();
		var response = await client.PostAsJsonAsync($"{Endpoints.TrainingApi}", updateTrainingCommand);
		var readTrainingDto = await response.Content.ReadFromJsonAsync<ReadTrainingDto>();

		response.StatusCode.Should().Be(HttpStatusCode.OK);
		readTrainingDto.Should().BeEquivalentTo(updateTrainingCommand);
	}

	[Fact]
	public async Task Post_ShortTrainingName_ReturnsBadRequest()
	{
		var command = new UpdateTrainingCommandFaker().Generate();
		command.Name = "1";
		var response = await client.PostAsJsonAsync($"{Endpoints.TrainingApi}", command);

		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task Post_EmptyTrainingDecription_ReturnsBadRequest()
	{
		var command = new UpdateTrainingCommandFaker().Generate();
		command.Description = "";
		var response = await client.PostAsJsonAsync($"{Endpoints.TrainingApi}", command);

		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}
}