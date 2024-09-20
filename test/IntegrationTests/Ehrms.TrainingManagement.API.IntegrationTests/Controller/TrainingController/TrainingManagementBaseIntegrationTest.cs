using Ehrms.Shared;
using System.Net.Http.Headers;
using Ehrms.Training.TestHelpers.Fakers.Models;
using Ehrms.TrainingManagement.API.Database.Context;

namespace Ehrms.TrainingManagement.API.IntegrationTests.Controller.TrainingController;

[Collection(nameof(TrainingManagementWebApplicationFactory))]
public abstract class TrainingManagementBaseIntegrationTest : IAsyncLifetime
{
    private readonly Func<Task> _resetDatabase;

    protected readonly HttpClient client;
    protected readonly TrainingDbContext dbContext;

	protected TrainingManagementBaseIntegrationTest(TrainingManagementWebApplicationFactory factory)
	{
		client = factory.CreateClient();
        _resetDatabase = factory.ResetDatabaseAsync;
         dbContext = factory.CreateDbContext();

        var request = new GenerateJwtRequest
		{
			Username = "TestUser",
		};
		var jwt = new JwtTokenHandler().Generate(request);
		client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt!.AccessToken);
	}

	protected async Task<Database.Models.Training> InsertRandomTraningRecord()
	{
		var training = new TrainingFaker().Generate();
		await dbContext.AddAsync(training);
		await dbContext.SaveChangesAsync();
		return training;
	}

    public Task InitializeAsync() => _resetDatabase();
    public Task DisposeAsync() => Task.CompletedTask;
}