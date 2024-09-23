using System.Net.Http.Headers;
using Ehrms.Shared;
using Ehrms.Training.TestHelpers.Fakers.Models;

namespace Ehrms.TrainingManagement.API.IntegrationTests.Controller.TrainingController;

[Collection(nameof(TrainingManagementWebApplicationFactory))]
public abstract class TrainingManagementBaseIntegrationTest : IAsyncLifetime
{
    private readonly Func<Task> _resetDatabase;
    private readonly TrainingManagementWebApplicationFactory _factory;
	protected readonly HttpClient client;

	protected TrainingManagementBaseIntegrationTest(TrainingManagementWebApplicationFactory factory)
	{
		_factory = factory;
		client = this._factory.CreateClient();
        _resetDatabase = factory.ResetDatabaseAsync;

        var request = new GenerateJwtRequest
		{
			Username = "TestUser",
		};
		var jwt = new JwtTokenHandler().Generate(request);
		client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt!.AccessToken);
	}

	protected async Task<Database.Models.Training> InsertRandomTraningRecord()
	{
		var dbContext = _factory.CreateDbContext();
		var training = new TrainingFaker().Generate();
		await dbContext.AddAsync(training);
		await dbContext.SaveChangesAsync();
		return training;
	}

    public Task InitializeAsync() => _resetDatabase();
    public Task DisposeAsync() => Task.CompletedTask;
}