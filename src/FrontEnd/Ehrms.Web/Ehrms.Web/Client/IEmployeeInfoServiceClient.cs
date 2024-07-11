using BitzArt.Blazor.Cookies;
using Ehrms.Web.Models;
using Ehrms.Web.Routing;
using System.Net;
using System.Net.Http.Headers;

namespace Ehrms.Web.Client;

public interface IEmployeeServiceClient
{
	Task CreateEmployeeAsync(Employee employee);
	Task<Employee> UpdateEmployeeAsync(Employee employee);
	Task DeleteEmployeeAsync(Guid id);
	Task<Employee> GetEmployeeAsync(Guid id);
	Task<IEnumerable<Employee>> GetEmployeesAsync(int pageNumber, int pageSize = 20);
}

public sealed class Response<T>
{
	public HttpStatusCode StatusCode { get; set; }
	public T? Content { get; set; }
}

public interface ISkillServiceClient
{
	Task<Response<IEnumerable<SkillModel>>> GetSkillsAsync();
}

public interface IHttpClientFactoryWrapper
{
	Task<HttpClient> CreateClient(string name);
}

internal sealed class HttpClientFactoryWrapper : IHttpClientFactoryWrapper
{
	private readonly ICookieService _cookieService;
	private readonly IHttpClientFactory _httpClientFactory;

	public HttpClientFactoryWrapper(IHttpClientFactory httpClientFactory, ICookieService cookieService)
	{
		_cookieService = cookieService;
		_httpClientFactory = httpClientFactory;
	}

	public async Task<HttpClient> CreateClient(string name)
	{
		var client = _httpClientFactory.CreateClient(name);
		var accessToken = await _cookieService.GetAsync(CookieKeys.AccessToken);
		client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken?.Value);

		return client;
	}
}

internal sealed class SkillServiceClient : ISkillServiceClient
{
	private readonly IEndpointProvider _endpointProvider;
	private readonly IHttpClientFactoryWrapper _clientFactoryWrapper;

	public SkillServiceClient(IHttpClientFactoryWrapper clientFactoryWrapper, IEndpointProvider endpointProvider)
	{
		_clientFactoryWrapper = clientFactoryWrapper;
		_endpointProvider = endpointProvider;
	}

	public async Task<Response<IEnumerable<SkillModel>>> GetSkillsAsync()
	{
		var client = await _clientFactoryWrapper.CreateClient("ApiGateway");
		var response = await client.GetAsync(_endpointProvider.EmployeeInfoServiceEndpoint);

		return new Response<IEnumerable<SkillModel>>()
		{
			StatusCode = response.StatusCode,
			Content = await response.GetContentAs<IEnumerable<SkillModel>>()
		};
	}
}

internal static class HttpResponseExtensions
{
	internal static async Task<T?> GetContentAs<T>(this HttpResponseMessage httpResponseMessage)
	{
		try
		{
			return await httpResponseMessage.Content.ReadFromJsonAsync<T>();
		}
		catch
		{
			return default;
		}
	}
}