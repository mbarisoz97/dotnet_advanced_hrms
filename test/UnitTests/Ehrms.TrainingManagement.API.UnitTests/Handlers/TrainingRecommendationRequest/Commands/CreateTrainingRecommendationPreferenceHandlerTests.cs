using Ehrms.TrainingManagement.API.Handlers.Recommendation.Commands;

namespace Ehrms.TrainingManagement.API.UnitTests.Handlers.TrainingRecommendationRequest.Commands;
public class CreateTrainingRecommendationPreferenceHandlerTests
{
    private readonly TrainingDbContext _dbContext;
    private readonly CreateTrainingRecommendationPreferenceHandler _handler;
    public CreateTrainingRecommendationPreferenceHandlerTests()
    {
        _dbContext = TestDbContextFactory.CreateDbContext(Guid.NewGuid().ToString());
        _handler = new(_dbContext);
    }

    [Fact]
    public async Task Handle_NonExistingProjectId_ReturnsResultWithProjectNotFoundException()
    {
        var command = new CreateTrainingRecommendationPreferenceFaker().Generate();
        var commandResult = await _handler.Handle(command, default);
        var expectionInResult = commandResult.Match<Exception?>(_ => null, f => f);

        expectionInResult.Should().BeOfType<ProjectNotFoundException>();
    }

    [Fact]
    public async Task Handle_NonExistingTitleId_ReturnsResultWithTitleNotFoundException()
    {
        var project = new ProjectFaker().Generate();
        await _dbContext.AddAsync(project);
        await _dbContext.SaveChangesAsync();

        var command = new CreateTrainingRecommendationPreferenceFaker()
            .WithProjectId(project.Id)
            .Generate();
        var commandResult = await _handler.Handle(command, default);
        var expectionInResult = commandResult.Match<Exception?>(_ => null, f => f);

        expectionInResult.Should().BeOfType<TitleNotFoundException>();
    }

    [Fact]
    public async Task Handle_NonExistingSkillId_ReturnsResultWithSkillNotFoundException()
    {
        var project = new ProjectFaker().Generate();
        await _dbContext.AddAsync(project);
        var title = new TitleFaker().Generate();
        await _dbContext.AddAsync(title);
        await _dbContext.SaveChangesAsync();

        var command = new CreateTrainingRecommendationPreferenceFaker()
            .WithProjectId(project.Id)
            .WithTitleId(title.Id)
            .WithSkills([Guid.Empty])
            .Generate();

        var commandResult = await _handler.Handle(command, default);
        var expectionInResult = commandResult.Match<Exception?>(_ => null, f => f);

        expectionInResult.Should().BeOfType<SkillNotFoundException>();
    }

    [Fact]
    public async Task Handle_ValidCommandParameters_CreatesTrainingRecommendationPreference()
    {
        var project = new ProjectFaker().Generate();
        await _dbContext.AddAsync(project);
        var title = new TitleFaker().Generate();
        await _dbContext.AddAsync(title);
        var skills = new SkillFaker().Generate(2);
        await _dbContext.AddRangeAsync(skills);
        await _dbContext.SaveChangesAsync();

        var command = new CreateTrainingRecommendationPreferenceFaker()
           .WithProjectId(project.Id)
           .WithTitleId(title.Id)
           .WithSkills(skills.Select(x => x.Id).ToList())
           .Generate();

        var commandResult = await _handler.Handle(command, default);
        var preferenceInResult = commandResult.Match<TrainingRecommendationPreferences?>(s => s, _ => null);

        preferenceInResult!.Project.Should().Be(project);
        preferenceInResult!.Title.Should().Be(title);
        foreach (var skill in skills)
        {
            preferenceInResult.Skills.Should().ContainEquivalentOf(skill);
        }
    }
}