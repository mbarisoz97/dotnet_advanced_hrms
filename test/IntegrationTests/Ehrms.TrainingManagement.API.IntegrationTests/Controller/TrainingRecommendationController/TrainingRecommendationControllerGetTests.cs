using System.Net;
using System.Net.Http.Json;
using Ehrms.Training.TestHelpers.Fakers.Events;
using Ehrms.Training.TestHelpers.Fakers.Models;
using Ehrms.TrainingManagement.API.Dtos.RecommendationPreference;
using Ehrms.TrainingManagement.API.IntegrationTests.Controller.TrainingController;

namespace Ehrms.TrainingManagement.API.IntegrationTests.Controller.TrainingRecommendationController;

public class TrainingRecommendationControllerGetTests : TrainingManagementBaseIntegrationTest
{
    private const string RecommendationPreferenceEndpoint = "RecommendationPreferences";

    public TrainingRecommendationControllerGetTests(TrainingManagementWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Get_ReturnsAllRecommendationPreferences()
    {
        var project = new ProjectFaker().Generate();
        await dbContext.AddAsync(project);
        var title = new TitleFaker().Generate();
        await dbContext.AddAsync(title);
        var skills = new SkillFaker().Generate(2);
        await dbContext.AddRangeAsync(skills);
        var preferences = new TrainingRecommendationPreferencesFaker()
            .WithProject(project)
            .WithTitle(title)
            .WithSkills(skills)
            .Generate(2);
        await dbContext.AddRangeAsync(preferences);
        await dbContext.SaveChangesAsync();

        var response = await client.GetAsync($"{Endpoints.TrainingRecommendationApi}/{RecommendationPreferenceEndpoint}");
        var preferenceDtos = await response.Content.ReadFromJsonAsync<IEnumerable<ReadRecommendationPreferenceDto>>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        preferenceDtos!.Count().Should().BeGreaterThanOrEqualTo(preferences.Count);
    }

    [Fact]
    public async Task Get_NonExistingPreferenceId_ReturnsBadRequest()
    {
        var response = await client.GetAsync($"{Endpoints.TrainingRecommendationApi}/{RecommendationPreferenceEndpoint}/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Get_NonExistingPreferenceId_ReturnsOkWithPreferenceDto()
    {
        var project = new ProjectFaker().Generate();
        await dbContext.AddAsync(project);
        var title = new TitleFaker().Generate();
        await dbContext.AddAsync(title);
        var skills = new SkillFaker().Generate(2);
        await dbContext.AddRangeAsync(skills);
        var preference = new TrainingRecommendationPreferencesFaker()
            .WithProject(project)
            .WithTitle(title)
            .WithSkills(skills)
            .Generate();
        await dbContext.AddRangeAsync(preference);
        await dbContext.SaveChangesAsync();

        var response = await client.GetAsync($"{Endpoints.TrainingRecommendationApi}/{RecommendationPreferenceEndpoint}/{preference.Id}");
        var preferenceDto = await response.Content.ReadFromJsonAsync<ReadRecommendationPreferenceDto>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        preferenceDto!.Should().BeEquivalentTo(preference,
            opt => opt.ExcludingMissingMembers());
    }
}