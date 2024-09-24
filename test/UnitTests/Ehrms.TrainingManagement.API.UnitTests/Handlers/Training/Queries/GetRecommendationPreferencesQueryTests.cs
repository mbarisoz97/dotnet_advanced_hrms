using Ehrms.TrainingManagement.API.UnitTests.Consumers.Training;
using Ehrms.TrainingManagement.API.Handlers.Recommendation.Queries;

namespace Ehrms.TrainingManagement.API.UnitTests.Handlers.Training.Queries;

public class GetRecommendationPreferencesQueryTests
{
    private readonly TrainingDbContext _dbContext;
    private readonly GetRecommendationPreferencesQueryHandler _handler;

    public GetRecommendationPreferencesQueryTests()
    {
        _dbContext = TestDbContextFactory.CreateDbContext();
        _handler = new GetRecommendationPreferencesQueryHandler(_dbContext);
    }

    [Fact]
    public async Task Handle_RetursAllExistingPreferenceRecords()
    {
        var project = new ProjectFaker().Generate();
        await _dbContext.AddAsync(project);

        var title = new TitleFaker().Generate();
        await _dbContext.AddAsync(title);

        var skills = new SkillFaker().Generate(2);
        await _dbContext.AddRangeAsync(skills);

        var preferences = new TrainingRecommendationPreferencesFaker()
            .WithProject(project)
            .WithTitle(title)
            .WithSkills(skills)
            .Generate(1);

        await _dbContext.AddRangeAsync(preferences);
        await _dbContext.SaveChangesAsync();

        GetRecommendationPreferencesQuery query = new();
        await _handler.Handle(query, default);

        var preferenceInDatabase = _dbContext.TrainingRecommendationPreferences.ToList();
        preferenceInDatabase.Should().BeEquivalentTo(preferences);
    }
}