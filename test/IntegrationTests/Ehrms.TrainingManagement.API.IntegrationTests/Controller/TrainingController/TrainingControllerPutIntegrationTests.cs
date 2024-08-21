using System.Net;
using System.Net.Http.Json;
using Ehrms.Training.TestHelpers.Fakers.Commands;

namespace Ehrms.TrainingManagement.API.IntegrationTests.Controller.TrainingController;

public class TrainingControllerPutIntegrationTests : TrainingManagementBaseIntegrationTest
{
	public TrainingControllerPutIntegrationTests(TrainingManagementWebApplicationFactory factory) : base(factory)
	{
	}

	[Fact]
	public async Task Put_ValidTrainingDetails_ReturnsOkWithReadTrainingDto()
	{
		var createTrainingCommand = new CreateTrainingCommandFaker().Generate();
		var response = await client.PutAsJsonAsync(Endpoints.TrainingApi, createTrainingCommand);

		response.StatusCode.Should().Be(HttpStatusCode.OK);

		var readTrainingDto = await response.Content.ReadFromJsonAsync<ReadTrainingDto>();

		readTrainingDto.Should().BeEquivalentTo(createTrainingCommand);
	}

    [Fact]
    public async Task Put_ShortTrainingName_ReturnsBadRequest()
    {
        var command = new CreateTrainingCommandFaker().Generate();
		command.Name = "s";
		var response = await client.PutAsJsonAsync(Endpoints.TrainingApi, command);

		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task Put_EmptyDescription_ReturnsBadRequest()
	{
		var command = new CreateTrainingCommandFaker()
			.WithDescription("")
			.Generate();

		var response = await client.PutAsJsonAsync(Endpoints.TrainingApi, command);
		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task Put_EndDateIsACloserThanStartDate_ReturnsBadRequest()
	{
		var command = new CreateTrainingCommandFaker()
			.WithStartDate(DateTime.Now.AddHours(1))
			.WithEndDate(DateTime.Now)
			.Generate();

		var response = await client.PutAsJsonAsync(Endpoints.TrainingApi, command);
		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}
}
