using Ehrms.Shared;
using Ehrms.Web.Models;
using System.Net;
using System.Net.Http.Headers;

namespace Ehrms.Web.Client;

internal sealed class EmployeeInfoServiceClient : IEmployeeInfoServiceClient
{
	private const string EmployeeInfoServiceEndpoint = "/api/Employee";
	private readonly IHttpClientFactory _httpClientFactory;

	public EmployeeInfoServiceClient(IHttpClientFactory httpClientFactory)
	{
		_httpClientFactory = httpClientFactory;
	}

	public async Task<IEnumerable<Employee>> GetEmployees()
	{
		var client = _httpClientFactory.CreateClient("ApiGateway");
		var authenticationResponse = await client.PostAsJsonAsync("/api/Account", new
		{
			Username = "Test",
			Password = "Test"
		});
		var jwt = await authenticationResponse.Content.ReadFromJsonAsync<GenerateTokenResponse>();

		client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt!.Token);
		var response = await client.GetAsync(EmployeeInfoServiceEndpoint);

		if (response.StatusCode == HttpStatusCode.OK)
		{
			return await response.Content.ReadFromJsonAsync<IEnumerable<Employee>>() ?? [];
		}
		
		return [];
	}
}