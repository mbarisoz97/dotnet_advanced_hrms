using Ehrms.TrainingManagement.API.Handlers.Recommendation.Commands;

namespace Ehrms.TrainingManagement.API.UnitTests.Handlers.TrainingRecommendationRequest.Commands;

public class UpdateTrainingRecommendationPreferenceHandlerTests
{
    private readonly TrainingDbContext _dbContext;
    private readonly UpdateTrainingRecommendationPreferenceCommandHandler _handler;

    public UpdateTrainingRecommendationPreferenceHandlerTests()
    {
        _dbContext = TestDbContextFactory.CreateDbContext();
        _handler = new UpdateTrainingRecommendationPreferenceCommandHandler(_dbContext);
    }

    [Fact]
    public async Task Handle_NonExistingPreferenceId_ReturnsResultWithTrainingRecommendationPreferenceNotFoundException()
    {
        var command = new UpdateTrainingRecommendationPreferenceFaker().Generate();
        var commandResult = await _handler.Handle(command, default);
        var expectionInResult = commandResult.Match<Exception?>(_ => null, f => f);

        expectionInResult.Should().BeOfType<TrainingRecommendationPreferenceNotFoundException>();
    }

    [Fact]
    public async Task Handle_NonExistingSkillId_ReturnsResultWithSkillNotFoundException()
    {
        var project = new ProjectFaker().Generate();
        await _dbContext.AddAsync(project);
        var title = new TitleFaker().Generate();
        await _dbContext.AddAsync(title);
        var skills = new SkillFaker().Generate(2);
        var preference = new TrainingRecommendationPreferencesFaker()
            .WithProject(project)
            .WithTitle(title)
            .WithSkills(skills)
            .Generate();
        await _dbContext.AddAsync(preference);
        await _dbContext.SaveChangesAsync();

        var command = new UpdateTrainingRecommendationPreferenceFaker()
            .WithId(preference.Id)
            .WithSkills([Guid.NewGuid()])
            .Generate();
        var commandResult = await _handler.Handle(command, default);
        var expectionInResult = commandResult.Match<Exception?>(_ => null, f => f);

        expectionInResult.Should().BeOfType<SkillNotFoundException>();
    }

    [Fact]
    public async Task Handle_ValidParameters_UpdatesPreferenceDetails()
    {
        var project = new ProjectFaker().Generate();
        await _dbContext.AddAsync(project);
        var title = new TitleFaker().Generate();
        await _dbContext.AddAsync(title);
        var skillsBeforeUpdate = new SkillFaker().Generate(2);
        await _dbContext.AddRangeAsync(skillsBeforeUpdate);
        var skillsAfterUpdate = new SkillFaker().Generate(2);
        await _dbContext.AddRangeAsync(skillsAfterUpdate);

        var preference = new TrainingRecommendationPreferencesFaker()
            .WithProject(project)
            .WithTitle(title)
            .WithSkills(skillsBeforeUpdate)
            .Generate();

        await _dbContext.AddAsync(preference);
        await _dbContext.SaveChangesAsync();

        var command = new UpdateTrainingRecommendationPreferenceFaker()
            .WithId(preference.Id)
            .WithSkills(skillsAfterUpdate)
            .Generate();

        var commandResult = await _handler.Handle(command, default);
        var preferenceInResult = commandResult.Match<TrainingRecommendationPreferences?>(s => s, _ => null);

        preferenceInResult!.Should().BeEquivalentTo(preference,
            opt => opt.Excluding(p=>p.Skills));
        preferenceInResult!.Skills.Should().BeEquivalentTo(skillsAfterUpdate);
    }
}