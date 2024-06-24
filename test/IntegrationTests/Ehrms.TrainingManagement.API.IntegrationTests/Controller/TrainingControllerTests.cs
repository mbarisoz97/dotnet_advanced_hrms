using System.Net.Http.Json;

namespace Ehrms.TrainingManagement.API.IntegrationTests.Controller;

public class TrainingControllerTests
{
    [Fact]
    public async Task Put_ValidTrainingDetails_CreatesNewTrainingRecord()
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
}