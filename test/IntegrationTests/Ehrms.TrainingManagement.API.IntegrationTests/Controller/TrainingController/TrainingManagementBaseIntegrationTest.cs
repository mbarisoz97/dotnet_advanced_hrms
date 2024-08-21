using System.Net.Http.Headers;
using Ehrms.Shared;
using Ehrms.Training.TestHelpers.Fakers.Models;
using Ehrms.TrainingManagement.API.Database.Models;

namespace Ehrms.TrainingManagement.API.IntegrationTests.Controller.TrainingController;

public abstract class TrainingManagementBaseIntegrationTest : IClassFixture<TrainingManagementWebApplicationFactory>
{
	private readonly TrainingManagementWebApplicationFactory _factory;
	protected readonly HttpClient client;

	protected TrainingManagementBaseIntegrationTest(TrainingManagementWebApplicationFactory factory)
	{
		_factory = factory;
		client = this._factory.CreateClient();

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
}