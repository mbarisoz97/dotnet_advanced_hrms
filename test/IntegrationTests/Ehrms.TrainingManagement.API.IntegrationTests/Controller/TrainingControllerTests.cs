using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Ehrms.Shared;
using Ehrms.TrainingManagement.API.Models;

namespace Ehrms.TrainingManagement.API.IntegrationTests.Controller;

public class TrainingControllerTests : IClassFixture<TrainingManagementWebApplicationFactory>
{
    private readonly TrainingManagementWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public TrainingControllerTests(TrainingManagementWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();

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
        var createTrainingCommand = new CreateTrainingCommandFaker().Generate();

        var response = await _client.PutAsJsonAsync(Endpoints.TrainingApi, createTrainingCommand);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var readTrainingDto = await response.Content.ReadFromJsonAsync<ReadTrainingDto>();

        readTrainingDto.Should().BeEquivalentTo(createTrainingCommand);
    }

    [Fact]
    public async Task Get_ExistingTrainingId_ReturnsOkWithReadTrainingDto()
    {
        Training traininig = await InsertRandomTraningRecord();

        var response = await _client.GetAsync($"{Endpoints.TrainingApi}/{traininig.Id}");
        var readTrainingResponse = await response.Content.ReadFromJsonAsync<ReadTrainingDto>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        readTrainingResponse?.Should().BeEquivalentTo(traininig);
    }

    [Fact]
    public async Task Get_ReturnsOkWithCollectionOfReadTrainingDtos()
    {
        await InsertRandomTraningRecord();

        var response = await _client.GetAsync($"{Endpoints.TrainingApi}");
        var readTrainingResponse = await response.Content.ReadFromJsonAsync<List<ReadTrainingDto>>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        readTrainingResponse.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Delete_ExistingTrainingId_ReturnsNoContent()
    {
        Training traininig = await InsertRandomTraningRecord();

        var response = await _client.DeleteAsync($"{Endpoints.TrainingApi}/{traininig.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Post_ExistingTrainingIdWithValidDetails_ReturnsOkWithReadTrainingDto()
    {
        Training traininig = await InsertRandomTraningRecord();

        var updateTrainingCommand = new UpdateTrainingCommandFaker()
            .WithId(traininig.Id)
            .Generate();
        var response = await _client.PostAsJsonAsync($"{Endpoints.TrainingApi}", updateTrainingCommand);
        var readTrainingDto = await response.Content.ReadFromJsonAsync<ReadTrainingDto>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        readTrainingDto.Should().BeEquivalentTo(updateTrainingCommand);
    }

    private async Task<Training> InsertRandomTraningRecord()
    {
        var dbContext = _factory.CreateDbContext();
        var traininig = new TrainingFaker().Generate();
        await dbContext.AddAsync(traininig);
        await dbContext.SaveChangesAsync();
        return traininig;
    }
}