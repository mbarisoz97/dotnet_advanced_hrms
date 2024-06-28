using Ehrms.Shared;
using System.Net.Http.Headers;

namespace Ehrms.EmployeeInfo.API.IntegrationTests.Controllers;

public abstract class BaseEmployeeInfoIntegrationTest : IClassFixture<EmployeeInfoWebApplicationFactory>
{
	protected readonly EmployeeInfoWebApplicationFactory factory;
	protected readonly HttpClient client;

	protected BaseEmployeeInfoIntegrationTest(EmployeeInfoWebApplicationFactory factory)
	{
		this.factory = factory;
		client = this.factory.CreateClient();

		var request = new AuthenticationRequest
		{
			Username = "TestUser",
			Password = "TestPassword"
		};
		var jwt = new JwtTokenHandler().Generate(request);
		client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt!.Token);
		this.factory = factory;
	}
}
