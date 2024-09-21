using Ehrms.TrainingManagement.API.Handlers.Recommendation.Commands;

namespace Ehrms.TrainingManagement.API.UnitTests.Handlers.TrainingRecommendationRequest.Commands;

public class DeleteTrainingRecommendationPreferenceHandlerTests
{
    private readonly TrainingDbContext _dbContext;
    private readonly DeleteTrainingRecommendationPreferenceCommandHandler _handler;

    public DeleteTrainingRecommendationPreferenceHandlerTests()
    {
        _dbContext = TestDbContextFactory.CreateDbContext();
        _handler = new DeleteTrainingRecommendationPreferenceCommandHandler(_dbContext);
    }

    [Fact]
    public async Task Handle_NonExistingPreferenceId_ReturnsResultWithTrainingRecommendationPreferenceNotFoundException()
    {
        var command = new DeleteTrainingRecommendationPreferenceCommandFaker().Generate();
        var commandResult = await _handler.Handle(command, default);
        var expectionInResult = commandResult.Match<Exception?>(_ => null, f => f);

        expectionInResult.Should().BeOfType<TrainingRecommendationPreferenceNotFoundException>();
    }

    [Fact]
    public async Task Handle_ExistingPreferenceId_RemovePreferenceRecord()
    {
        var project = new ProjectFaker().Generate();
        await _dbContext.AddAsync(project);
        var title = new TitleFaker().Generate();
        await _dbContext.AddAsync(title);
        var preference = new TrainingRecommendationPreferencesFaker().Generate();
        await _dbContext.AddAsync(preference);
        await _dbContext.SaveChangesAsync();

        var command = new DeleteTrainingRecommendationPreferenceCommandFaker()
            .WithId(preference.Id)
            .Generate();
        await _handler.Handle(command, default);

        _dbContext.TrainingRecommendationPreferences
            .Count().Should().Be(0);    
    }
}