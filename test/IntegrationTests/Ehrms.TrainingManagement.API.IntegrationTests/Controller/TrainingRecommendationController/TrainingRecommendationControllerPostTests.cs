using System.Net;
using System.Net.Http.Json;
using Ehrms.Training.TestHelpers.Fakers.Events;
using Ehrms.Training.TestHelpers.Fakers.Models;
using Ehrms.TrainingManagement.API.Dtos.RecommendationPreference;
using Ehrms.TrainingManagement.API.IntegrationTests.Controller.TrainingController;

namespace Ehrms.TrainingManagement.API.IntegrationTests.Controller.TrainingRecommendationController;

public class TrainingRecommendationControllerPostTests : TrainingManagementBaseIntegrationTest
{
    private const string RecommendationPreferenceEndpoint = "RecommendationPreferences";

    public TrainingRecommendationControllerPostTests(TrainingManagementWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Post_NonExistingPreferenceId_ReturnsBadRequest()
    {
        var updatePreferenceCommand = new UpdateTrainingRecommendationPreferenceFaker()
            .WithId(Guid.NewGuid())
            .Generate();

        var response = await client.PostAsJsonAsync($"{Endpoints.TrainingRecommendationApi}/{RecommendationPreferenceEndpoint}", updatePreferenceCommand);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Post_EmptySkillList_ReturnsBadRequest()
    {
        var updatePreferenceCommand = new UpdateTrainingRecommendationPreferenceFaker()
            .WithId(Guid.NewGuid())
            .WithSkills(Array.Empty<Guid>())
            .Generate();

        var response = await client.PostAsJsonAsync($"{Endpoints.TrainingRecommendationApi}/{RecommendationPreferenceEndpoint}", updatePreferenceCommand);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Post_EmptyPreferenceId_ReturnsBadRequest()
    {
        var updatePreferenceCommand = new UpdateTrainingRecommendationPreferenceFaker()
            .WithId(Guid.Empty)
            .WithSkills([Guid.NewGuid()])
            .Generate();

        var response = await client.PostAsJsonAsync($"{Endpoints.TrainingRecommendationApi}/{RecommendationPreferenceEndpoint}", updatePreferenceCommand);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Post_NonExistingTitleId_ReturnsBadRequest()
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

        await dbContext.AddAsync(preference);
        await dbContext.SaveChangesAsync();

        var updatePreferenceCommand = new UpdateTrainingRecommendationPreferenceFaker()
            .WithId(Guid.NewGuid())
            .WithSkills([Guid.NewGuid()])
            .Generate();

        var response = await client.PostAsJsonAsync($"{Endpoints.TrainingRecommendationApi}/{RecommendationPreferenceEndpoint}", updatePreferenceCommand);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Post_ValidUpdateCommand_ReturnsNoContent()
    {
        var project = new ProjectFaker().Generate();
        await dbContext.AddAsync(project);
        var title = new TitleFaker().Generate();
        await dbContext.AddAsync(title);
        var skillsBeforeUpdate = new SkillFaker().Generate(2);
        await dbContext.AddRangeAsync(skillsBeforeUpdate);
        var skillsAfterUpdate = new SkillFaker().Generate(2);
        await dbContext.AddRangeAsync(skillsAfterUpdate);

        var preference = new TrainingRecommendationPreferencesFaker()
            .WithProject(project)
            .WithTitle(title)
            .WithSkills(skillsBeforeUpdate)
            .Generate();

        await dbContext.AddAsync(preference);
        await dbContext.SaveChangesAsync();

        var updatePreferenceCommand = new UpdateTrainingRecommendationPreferenceFaker()
            .WithId(preference.Id)
            .WithSkills(skillsAfterUpdate)
            .Generate();

        var response = await client.PostAsJsonAsync($"{Endpoints.TrainingRecommendationApi}/{RecommendationPreferenceEndpoint}", updatePreferenceCommand);
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}