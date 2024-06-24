using Ehrms.TrainingManagement.API.Models;
using System.Net;
using System.Net.Http.Json;

namespace Ehrms.TrainingManagement.API.IntegrationTests.Controller;

public class TrainingControllerTests
{
    [Fact]
    public async Task Put_ValidTrainingDetails_ReturnsOkWithReadTrainingDto()
    {
        TrainingManagementWebApplicationFactory application = new();
        CreateTrainingDto createTrainingDto = new CreateTrainingDtoFaker().Generate();

        var client = application.CreateClient();
        var response = await client.PutAsJsonAsync(Endpoints.TrainingApi, createTrainingDto);
        var readTrainingDto = await response.Content.ReadFromJsonAsync<ReadTrainingDto>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        readTrainingDto.Should().BeEquivalentTo(createTrainingDto);
    }

    [Fact]
    public async Task Get_ExistingTrainingId_ReturnsOkWithReadTrainingDto()
    {
        Training traininig = await InsertRandomTraningRecord();

        TrainingManagementWebApplicationFactory application = new();
        var client = application.CreateClient();
        var response = await client.GetAsync($"{Endpoints.TrainingApi}/{traininig.Id}");
        var readTrainingResponse = await response.Content.ReadFromJsonAsync<ReadTrainingDto>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        readTrainingResponse?.Id.Should().Be(traininig.Id);
        readTrainingResponse?.Name.Should().Be(traininig.Name);
        readTrainingResponse?.Description.Should().Be(traininig.Description);
        readTrainingResponse?.PlannedAt.Should().Be(traininig.PlannedAt);
        readTrainingResponse?.Participants.Should().BeEmpty();
    }

    [Fact]
    public async Task Delete_ExistingTrainingId_ReturnsNoContent()
    {
        Training traininig = await InsertRandomTraningRecord();

        TrainingManagementWebApplicationFactory application = new();
        var client = application.CreateClient();
        var response = await client.DeleteAsync($"{Endpoints.TrainingApi}/{traininig.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Post_ExistingTrainingIdWithValidDetails_ReturnsOkWithReadTrainingDto()
    {
        Training traininig = await InsertRandomTraningRecord();

        TrainingManagementWebApplicationFactory application = new();
        var client = application.CreateClient();
        var updateTrainingDto = new UpdateTrainingDtoFaker()
            .WithId(traininig.Id)
            .Generate();
        var response = await client.PostAsJsonAsync($"{Endpoints.TrainingApi}", updateTrainingDto);
        var readTrainingDto = await response.Content.ReadFromJsonAsync<ReadTrainingDto>();
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        readTrainingDto.Should().BeEquivalentTo(updateTrainingDto);
    }

    private static async Task<Training> InsertRandomTraningRecord()
    {
        var dbContext = CustomDbContextFactory.Create(TrainingManagementWebApplicationFactory.DatabaseName);
        var traininig = new TrainingFaker().Generate();
        await dbContext.AddAsync(traininig);
        await dbContext.SaveChangesAsync();
        return traininig;
    }
}