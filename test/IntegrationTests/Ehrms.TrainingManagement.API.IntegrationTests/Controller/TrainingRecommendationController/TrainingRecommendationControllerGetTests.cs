using Ehrms.Training.TestHelpers.Fakers.Events;
using Ehrms.Training.TestHelpers.Fakers.Models;
using Ehrms.TrainingManagement.API.Database.Models;
using Ehrms.TrainingManagement.API.Dtos.RecommendationPreference;
using Ehrms.TrainingManagement.API.IntegrationTests.Controller.TrainingController;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net.Http.Json;

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
     
        preferenceDtos!.Count().Should().Be(preferences.Count);
    }
}