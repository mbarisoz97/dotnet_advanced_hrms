using Ehrms.TrainingManagement.API.Handlers.Recommendation.Queries;

namespace Ehrms.TrainingManagement.API.UnitTests.Handlers.Preference.Queries;

public class GetRecommendationPreferencesByIdQueryHandlerTests
{
    private readonly TrainingDbContext _dbContext;
    private readonly GetRecommendationPreferencesByIdQueryHandler _handler;

    public GetRecommendationPreferencesByIdQueryHandlerTests()
    {
        _dbContext = TestDbContextFactory.CreateDbContext();
        _handler = new GetRecommendationPreferencesByIdQueryHandler(_dbContext);
    }

    [Fact]
    public async Task Handle_NonExistingPreferenceId_ReturnsResultWithTrainingRecommendationPreferenceNotFoundException()
    {
        var query = new GetRecommendationPreferencesByIdQuery { Id = Guid.NewGuid() };
        var queryResult = await _handler.Handle(query, default);
        var expectionInResult = queryResult.Match<Exception?>(_ => null, f => f);

        expectionInResult.Should().BeOfType<TrainingRecommendationPreferenceNotFoundException>();
    }

    [Fact]
    public async Task Handle_ExistingPreferenceId_ReturnsPreferenceObject()
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

        var query = new GetRecommendationPreferencesByIdQuery { Id = preference.Id};
        var queryResult = await _handler.Handle(query, default);
        var preferenceInResult = queryResult.Match<TrainingRecommendationPreferences?>(s => s, _ => null);

        preferenceInResult.Should().BeEquivalentTo(preference, opt => opt.IgnoringCyclicReferences());
    }
}