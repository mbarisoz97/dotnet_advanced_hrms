using Ehrms.Web.Models;
using Ehrms.Web.Routing;

namespace Ehrms.Web.Client;

internal class EmploymentServiceClient : IEmploymentServiceClient
{
	private readonly IEndpointProvider _endpointProvider;
	private readonly IHttpClientFactoryWrapper _httpClientFactory;

	public EmploymentServiceClient(IEndpointProvider endpointProvider, IHttpClientFactoryWrapper httpClientFactory)
	{
		_endpointProvider = endpointProvider;
		_httpClientFactory = httpClientFactory;
	}

	public async Task<Response<IEnumerable<WorkerEmploymentModel>>> GetEmploymenHistoryByEmployeeId(Guid employeeId)
	{
		var client = await _httpClientFactory.CreateClient(HttpClients.BackendApiGateway);
		var response = await client.GetAsync($"{_endpointProvider.EmploymentApiEndpoint}/Employee/{employeeId}");

		return new()
		{
			StatusCode = response.StatusCode,
			Content = await response.GetContentAs<IEnumerable<WorkerEmploymentModel>>()
		};
	}
}