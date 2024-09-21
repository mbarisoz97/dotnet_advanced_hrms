using Ehrms.Training.TestHelpers.Fakers.Events;
using Ehrms.Training.TestHelpers.Fakers.Models;
using Ehrms.TrainingManagement.API.Database.Models;
using Microsoft.EntityFrameworkCore;
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

    [Fact]
    public async Task Delete_NonExistingPreferenceId_ReturnsBadRequest()
    {
        var response = await client.DeleteAsync($"{Endpoints.TrainingRecommendationApi}/RecommendationPreferences/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Delete_ExistingPreferenceId_ReturnsNoContent()
    {
        var project = new ProjectFaker().Generate();
        await dbContext.AddAsync(project);
        var title = new TitleFaker().Generate();
        await dbContext.AddAsync(title);
        var preference = new TrainingRecommendationPreferencesFaker().Generate();
        await dbContext.AddAsync(preference);
        await dbContext.SaveChangesAsync();

        var response = await client.DeleteAsync($"{Endpoints.TrainingRecommendationApi}/RecommendationPreferences/{preference.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
