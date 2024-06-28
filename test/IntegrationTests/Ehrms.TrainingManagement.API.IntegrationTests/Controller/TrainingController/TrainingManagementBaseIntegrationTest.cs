using System.Net.Http.Headers;
using Ehrms.Shared;
using Ehrms.TrainingManagement.API.Models;

namespace Ehrms.TrainingManagement.API.IntegrationTests.Controller.TrainingController;

public abstract class TrainingManagementBaseIntegrationTest : IClassFixture<TrainingManagementWebApplicationFactory>
{
	private readonly TrainingManagementWebApplicationFactory _factory;
	protected readonly HttpClient client;

	protected TrainingManagementBaseIntegrationTest(TrainingManagementWebApplicationFactory factory)
	{
		_factory = factory;
		client = this._factory.CreateClient();

		var request = new AuthenticationRequest
		{
			Username = "TestUser",
			Password = "TestPassword"
		};
		var jwt = new JwtTokenHandler().Generate(request);
		client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt!.Token);
	}

	protected async Task<Training> InsertRandomTraningRecord()
	{
		var dbContext = _factory.CreateDbContext();
		var traininig = new TrainingFaker().Generate();
		await dbContext.AddAsync(traininig);
		await dbContext.SaveChangesAsync();
		return traininig;
	}
}