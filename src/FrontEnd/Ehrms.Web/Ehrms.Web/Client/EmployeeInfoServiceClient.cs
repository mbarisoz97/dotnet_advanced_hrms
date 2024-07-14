using System.Net;
using Ehrms.Web.Models;
using BitzArt.Blazor.Cookies;
using System.Net.Http.Headers;
using Ehrms.Web.Routing;

namespace Ehrms.Web.Client;

internal sealed class EmployeeInfoServiceClient : IEmployeeServiceClient
{
	private const string EmployeeInfoServiceEndpoint = "/api/Employee";
	private readonly IHttpClientFactoryWrapper _factoryWrapper;
	private readonly ICookieService _cookieService;
	private readonly IHttpClientFactory _httpClientFactory;

	public EmployeeInfoServiceClient(IHttpClientFactoryWrapper factoryWrapper, ICookieService cookieService, IHttpClientFactory httpClientFactory)
	{
		_factoryWrapper = factoryWrapper;
		_cookieService = cookieService;
		_httpClientFactory = httpClientFactory;
	}

	public async Task CreateEmployeeAsync(EmployeeModel employee)
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

	public async Task<EmployeeModel> GetEmployeeAsync(Guid id)
	{
		var client = _httpClientFactory.CreateClient("ApiGateway");
		var token = await _cookieService.GetAsync(CookieKeys.AccessToken);
		client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
		var response = await client.GetAsync($"{EmployeeInfoServiceEndpoint}/{id}");

		return await response.Content.ReadFromJsonAsync<EmployeeModel>();
	}

	public async Task<EmployeeModel> UpdateEmployeeAsync(EmployeeModel employee)
	{
		var client = _httpClientFactory.CreateClient("ApiGateway");
		var token = await _cookieService.GetAsync(CookieKeys.AccessToken);
		client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
		var response = await client.PostAsJsonAsync(EmployeeInfoServiceEndpoint, employee);

		return await response.Content.ReadFromJsonAsync<EmployeeModel>();
	}

	public async Task<Response<IEnumerable<EmployeeModel>>> GetEmployeesAsync()
	{
		var client = await _factoryWrapper.CreateClient("ApiGateway");
		var response = await client.GetAsync(EmployeeInfoServiceEndpoint);

		return new()
		{
			StatusCode = response.StatusCode,
			Content = await response.GetContentAs<IEnumerable<EmployeeModel>>()
		};
	}
}