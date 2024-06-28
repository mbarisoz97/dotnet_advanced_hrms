using Ehrms.Shared;
using System.Net.Http.Headers;

namespace Ehrms.EmployeeInfo.API.IntegrationTests.Controllers;

public abstract class BaseEmployeeInfoIntegrationTest : IClassFixture<EmployeeInfoWebApplicationFactory>
{
	protected readonly EmployeeInfoWebApplicationFactory _factory;
	protected readonly HttpClient _client;

	protected BaseEmployeeInfoIntegrationTest(EmployeeInfoWebApplicationFactory factory)
	{
		_factory = factory;
		_client = _factory.CreateClient();

		var request = new AuthenticationRequest
		{
			Username = "TestUser",
			Password = "TestPassword"
		};
		var jwt = new JwtTokenHandler().Generate(request);
		_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt!.Token);
		_factory = factory;
	}
}
