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
        var createTrainingResponse = await response.Content.ReadFromJsonAsync<ReadTrainingDto>();

        createTrainingResponse?.Id.Should().NotBe(Guid.Empty);
        createTrainingResponse?.Name.Should().Be(createTrainingDto.Name);
        createTrainingResponse?.Description.Should().Be(createTrainingDto.Description);
        createTrainingResponse?.PlannedAt.Should().Be(createTrainingDto.PlannedAt);
        createTrainingResponse?.Participants.Should().BeEmpty();
    }

    [Fact]
    public async Task Get_ExistingTrainingId_ReturnsOkWithReadTrainingDto()
    {
        var dbContext = CustomDbContextFactory.Create(TrainingManagementWebApplicationFactory.DatabaseName);
        var traininig = new TrainingFaker().Generate();
        await dbContext.AddAsync(traininig);
        await dbContext.SaveChangesAsync();

        TrainingManagementWebApplicationFactory application = new();
        var client = application.CreateClient();
        var response = await client.GetAsync($"{Endpoints.TrainingApi}/{traininig.Id}");
        var readTrainingResponse = await response.Content.ReadFromJsonAsync<ReadTrainingDto>();

        readTrainingResponse?.Id.Should().Be(traininig.Id);
        readTrainingResponse?.Name.Should().Be(traininig.Name);
        readTrainingResponse?.Description.Should().Be(traininig.Description);
        readTrainingResponse?.PlannedAt.Should().Be(traininig.PlannedAt);
        readTrainingResponse?.Participants.Should().BeEmpty();
    }

    [Fact]
    public async Task Delete_ExistingTrainingId_ReturnsNoContent()
    {
        var dbContext = CustomDbContextFactory.Create(TrainingManagementWebApplicationFactory.DatabaseName);
        var traininig = new TrainingFaker().Generate();
        await dbContext.AddAsync(traininig);
        await dbContext.SaveChangesAsync();

        TrainingManagementWebApplicationFactory application = new();
        var client = application.CreateClient();
        var response = await client.DeleteAsync($"{Endpoints.TrainingApi}/{traininig.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}