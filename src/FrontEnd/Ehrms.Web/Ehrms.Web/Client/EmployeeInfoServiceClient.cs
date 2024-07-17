using Ehrms.Web.Models;

namespace Ehrms.Web.Client;

internal sealed class EmployeeInfoServiceClient : IEmployeeServiceClient
{
	private readonly IEndpointProvider _endpointProvider;
	private readonly IHttpClientFactoryWrapper _factoryWrapper;

	public EmployeeInfoServiceClient(IEndpointProvider endpointProvider, IHttpClientFactoryWrapper factoryWrapper)
	{
		_factoryWrapper = factoryWrapper;
		_endpointProvider = endpointProvider;
	}

	public async Task<Response<EmployeeModel>> CreateEmployeeAsync(EmployeeModel employee)
	{
		var client = await _factoryWrapper.CreateClient("ApiGateway");
		var response = await client.PutAsJsonAsync(_endpointProvider.EmployeeInfoServiceEndpoint, employee);

		return new()
		{
			StatusCode = response.StatusCode,
			Content = await response.GetContentAs<EmployeeModel>(),
		};
	}

	public async Task<Response<Guid>> DeleteEmployeeAsync(Guid id)
	{
		var client = await _factoryWrapper.CreateClient("ApiGateway");
		var response = await client.DeleteAsync($"{_endpointProvider.EmployeeInfoServiceEndpoint}/{id}");

		return new()
		{
			StatusCode = response.StatusCode,
			Content = await response.GetContentAs<Guid>()
		};
	}

	public async Task<Response<EmployeeModel>> GetEmployeeAsync(Guid id)
	{
		var client = await _factoryWrapper.CreateClient("ApiGateway");
		var response = await client.GetAsync($"{_endpointProvider.EmployeeInfoServiceEndpoint}/{id}");

		return new()
		{
			StatusCode = response.StatusCode,
			Content = await response.GetContentAs<EmployeeModel>()
		};
	}

	public async Task<Response<EmployeeModel>> UpdateEmployeeAsync(EmployeeModel employee)
	{
		var client = await _factoryWrapper.CreateClient("ApiGateway");
		var response = await client.PostAsJsonAsync(_endpointProvider.EmployeeInfoServiceEndpoint, employee);

		return new()
		{
			StatusCode = response.StatusCode,
			Content = await response.GetContentAs<EmployeeModel>()
		};
	}

	public async Task<Response<IEnumerable<EmployeeModel>>> GetEmployeesAsync()
	{
		var client = await _factoryWrapper.CreateClient("ApiGateway");
		var response = await client.GetAsync(_endpointProvider.EmployeeInfoServiceEndpoint);

		return new()
		{
			StatusCode = response.StatusCode,
			Content = await response.GetContentAs<IEnumerable<EmployeeModel>>()
		};
	}
}