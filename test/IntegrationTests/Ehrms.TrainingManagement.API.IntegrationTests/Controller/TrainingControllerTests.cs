using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Ehrms.Shared;
using Ehrms.TrainingManagement.API.Models;

namespace Ehrms.TrainingManagement.API.IntegrationTests.Controller;

public class TrainingControllerTests : IAsyncLifetime
{
    private readonly TrainingManagementWebApplicationFactory _application = new();
    private readonly HttpClient _client;

    public TrainingControllerTests()
    {
        _application = new();
        _client = _application.CreateClient();

        var request = new AuthenticationRequest
        {
            Username = "TestUser",
            Password = "TestPassword"
        };
        var jwt = new JwtTokenHandler().Generate(request);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt!.Token);
    }

    [Fact]
    public async Task Put_ValidTrainingDetails_ReturnsOkWithReadTrainingDto()
    {
        TrainingManagementWebApplicationFactory application = new();
        CreateTrainingDto createTrainingDto = new CreateTrainingDtoFaker().Generate();

        var response = await _client.PutAsJsonAsync(Endpoints.TrainingApi, createTrainingDto);
        var readTrainingDto = await response.Content.ReadFromJsonAsync<ReadTrainingDto>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        readTrainingDto.Should().BeEquivalentTo(createTrainingDto);
    }

    [Fact]
    public async Task Get_ExistingTrainingId_ReturnsOkWithReadTrainingDto()
    {
        Training traininig = await InsertRandomTraningRecord();

        TrainingManagementWebApplicationFactory application = new();
        var response = await _client.GetAsync($"{Endpoints.TrainingApi}/{traininig.Id}");
        var readTrainingResponse = await response.Content.ReadFromJsonAsync<ReadTrainingDto>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        readTrainingResponse?.Id.Should().Be(traininig.Id);
        readTrainingResponse?.Name.Should().Be(traininig.Name);
        readTrainingResponse?.Description.Should().Be(traininig.Description);
        readTrainingResponse?.PlannedAt.Should().Be(traininig.PlannedAt);
        readTrainingResponse?.Participants.Should().BeEmpty();
    }

    [Fact]
    public async Task Get_ReturnsOkWithCollectionOfReadTrainingDtos()
    {
        await InsertRandomTraningRecord();

        TrainingManagementWebApplicationFactory application = new();
        var response = await _client.GetAsync($"{Endpoints.TrainingApi}");
        var readTrainingResponse = await response.Content.ReadFromJsonAsync<List<ReadTrainingDto>>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        readTrainingResponse.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Delete_ExistingTrainingId_ReturnsNoContent()
    {
        Training traininig = await InsertRandomTraningRecord();

        TrainingManagementWebApplicationFactory application = new();
        var response = await _client.DeleteAsync($"{Endpoints.TrainingApi}/{traininig.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Post_ExistingTrainingIdWithValidDetails_ReturnsOkWithReadTrainingDto()
    {
        Training traininig = await InsertRandomTraningRecord();

        TrainingManagementWebApplicationFactory application = new();
        var updateTrainingDto = new UpdateTrainingDtoFaker()
            .WithId(traininig.Id)
            .Generate();
        var response = await _client.PostAsJsonAsync($"{Endpoints.TrainingApi}", updateTrainingDto);
        var readTrainingDto = await response.Content.ReadFromJsonAsync<ReadTrainingDto>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        readTrainingDto.Should().BeEquivalentTo(updateTrainingDto);
    }

    private async Task<Training> InsertRandomTraningRecord()
    {
        var dbContext = _application.CreateDbContext();
        var traininig = new TrainingFaker().Generate();
        await dbContext.AddAsync(traininig);
        await dbContext.SaveChangesAsync();
        return traininig;
    }

    public async Task InitializeAsync()
    {
        await _application.Start();
    }

    public async Task DisposeAsync()
    {
        await _application.Stop();
    }
}