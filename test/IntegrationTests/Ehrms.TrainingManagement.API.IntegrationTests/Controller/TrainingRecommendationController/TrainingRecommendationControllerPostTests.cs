using System.Net.Http.Json;
using Ehrms.Training.TestHelpers.Fakers.Models;
using Ehrms.Training.TestHelpers.Fakers.Events;
using Ehrms.TrainingManagement.API.Dtos.RecommendationPreference;
using Ehrms.TrainingManagement.API.IntegrationTests.Controller.TrainingController;
using System.Net;

namespace Ehrms.TrainingManagement.API.IntegrationTests.Controller.TrainingRecommendationController;

public class TrainingRecommendationControllerPostTests : TrainingManagementBaseIntegrationTest
{
    public TrainingRecommendationControllerPostTests(TrainingManagementWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Post_ExistingTrainingIdWithValidDetails_ReturnsOkWithReadRecommendationPreferenceDto()
    {
        var project = new ProjectFaker().Generate();
        await dbContext.AddAsync(project);
        var title = new TitleFaker().Generate();
        await dbContext.AddAsync(title);
        var skills = new SkillFaker().Generate(2);
        await dbContext.AddRangeAsync(skills);
        await dbContext.SaveChangesAsync();

        var command = new CreateTrainingRecommendationPreferenceFaker()
            .WithProjectId(project.Id)
            .WithTitleId(title.Id)
            .WithSkills(skills)
            .Generate();

        var response = await client.PostAsJsonAsync($"{Endpoints.TrainingRecommendationApi}/RecommendationPreferences", command);
        var readRecommendationPreferenceDto = await response.Content.ReadFromJsonAsync<ReadRecommendationPreferenceDto>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Post_EmptyProjectId_ReturnsBadRequest()
    {
        var title = new TitleFaker().Generate();
        await dbContext.AddAsync(title);
        var skills = new SkillFaker().Generate(2);
        await dbContext.AddRangeAsync(skills);
        await dbContext.SaveChangesAsync();

        var command = new CreateTrainingRecommendationPreferenceFaker()
            .WithProjectId(Guid.Empty)
            .WithTitleId(title.Id)
            .WithSkills(skills)
            .Generate();

        var response = await client.PostAsJsonAsync($"{Endpoints.TrainingRecommendationApi}/RecommendationPreferences", command);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Post_EmptyTitleId_ReturnsBadRequest()
    {
        var project = new ProjectFaker().Generate();
        await dbContext.AddAsync(project);
        var skills = new SkillFaker().Generate(2);
        await dbContext.AddRangeAsync(skills);
        await dbContext.SaveChangesAsync();

        var command = new CreateTrainingRecommendationPreferenceFaker()
            .WithProjectId(project.Id)
            .WithTitleId(Guid.Empty)
            .WithSkills(skills)
            .Generate();

        var response = await client.PostAsJsonAsync($"{Endpoints.TrainingRecommendationApi}/RecommendationPreferences", command);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Post_EmptySkillIdCollection_ReturnsBadRequest()
    {
        var project = new ProjectFaker().Generate();
        await dbContext.AddAsync(project);
        var title = new TitleFaker().Generate();
        await dbContext.AddAsync(title);
        await dbContext.SaveChangesAsync();

        var command = new CreateTrainingRecommendationPreferenceFaker()
            .WithProjectId(project.Id)
            .WithTitleId(title.Id)
            .WithSkills(Array.Empty<Guid>())
            .Generate();

        var response = await client.PostAsJsonAsync($"{Endpoints.TrainingRecommendationApi}/RecommendationPreferences", command);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}