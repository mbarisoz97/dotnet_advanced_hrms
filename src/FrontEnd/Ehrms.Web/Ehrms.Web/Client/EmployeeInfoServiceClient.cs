using System.Net;
using Ehrms.Web.Models;
using BitzArt.Blazor.Cookies;
using System.Net.Http.Headers;
using Ehrms.Web.Routing;

namespace Ehrms.Web.Client;

internal sealed class EmployeeInfoServiceClient : IEmployeeServiceClient
{
	private const string EmployeeInfoServiceEndpoint = "/api/Employee";
	private readonly ICookieService _cookieService;
	private readonly IHttpClientFactory _httpClientFactory;

	public EmployeeInfoServiceClient(ICookieService cookieService, IHttpClientFactory httpClientFactory)
	{
		_cookieService = cookieService;
		_httpClientFactory = httpClientFactory;
	}

	public async Task CreateEmployeeAsync(Employee employee)
	{
		var client = _httpClientFactory.CreateClient("ApiGateway");
		var token = await _cookieService.GetAsync(CookieKeys.AccessToken);
		client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
		var response = await client.PutAsJsonAsync(EmployeeInfoServiceEndpoint, employee);
	}

	public async Task DeleteEmployeeAsync(Guid id)
	{
		var client = _httpClientFactory.CreateClient("ApiGateway");
		var token = await _cookieService.GetAsync(CookieKeys.AccessToken);
		client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
		await client.DeleteAsync($"{EmployeeInfoServiceEndpoint}/{id}");
	}

	public async Task<Employee> GetEmployeeAsync(Guid id)
	{
		var client = _httpClientFactory.CreateClient("ApiGateway");
		var token = await _cookieService.GetAsync(CookieKeys.AccessToken);
		client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
		var response = await client.GetAsync($"{EmployeeInfoServiceEndpoint}/{id}");

		return await response.Content.ReadFromJsonAsync<Employee>();
	}

	public async Task<IEnumerable<Employee>> GetEmployeesAsync(int page, int pageSize = 20)
	{
		var client = _httpClientFactory.CreateClient("ApiGateway");
		var token = await _cookieService.GetAsync(CookieKeys.AccessToken);
		client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
		var response = await client.GetAsync(EmployeeInfoServiceEndpoint);

		IEnumerable<Employee> employees = [];
		if (response.StatusCode == HttpStatusCode.OK)
		{
			employees = await response.Content.ReadFromJsonAsync<IEnumerable<Employee>>() ?? [];
		}
		return employees.Take(pageSize);
	}

	public async Task<Employee> UpdateEmployeeAsync(Employee employee)
	{
		var client = _httpClientFactory.CreateClient("ApiGateway");
		var token = await _cookieService.GetAsync(CookieKeys.AccessToken);
		client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
		var response = await client.PostAsJsonAsync(EmployeeInfoServiceEndpoint, employee);
		
		return await response.Content.ReadFromJsonAsync<Employee>();
	}
}