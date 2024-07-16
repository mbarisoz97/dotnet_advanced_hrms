using Ehrms.Web.Models;

namespace Ehrms.Web.Client;

internal sealed class TrainingServiceClient : ITrainingServiceClient
{
	private readonly IEndpointProvider _endpointProvider;
	private readonly IHttpClientFactoryWrapper _httpClientFactoryWrapper;

	public TrainingServiceClient(IEndpointProvider endpointProvider, IHttpClientFactoryWrapper httpClientFactoryWrapper)
	{
		_endpointProvider = endpointProvider;
		_httpClientFactoryWrapper = httpClientFactoryWrapper;
	}

	public async Task<Response<Guid>> CreateTrainingAsync(TrainingModel trainingModel)
	{
		var client = await _httpClientFactoryWrapper.CreateClient("ApiGateway");
		var response = await client.PutAsJsonAsync(_endpointProvider.TrainingManagementServiceEndpoint, trainingModel);

		return new()
		{
			StatusCode = response.StatusCode,
			Content = await response.GetContentAs<Guid>()
		};
	}

	public async Task<Response<Guid>> DeleteTrainingAsync(Guid id)
	{
		var client = await _httpClientFactoryWrapper.CreateClient("ApiGateway");
		var response = await client.DeleteAsync($"{_endpointProvider.TrainingManagementServiceEndpoint}/{id}");

		return new()
		{
			StatusCode = response.StatusCode,
			Content = await response.GetContentAs<Guid>()
		};
	}

	public async Task<Response<TrainingModel>> GetTrainingAsync(Guid id)
	{
		var client = await _httpClientFactoryWrapper.CreateClient("ApiGateway");
		var response = await client.GetAsync($"{_endpointProvider.TrainingManagementServiceEndpoint}/{id}");

		return new()
		{
			StatusCode = response.StatusCode,
			Content = await response.GetContentAs<TrainingModel>()
		};
	}

	public async Task<Response<IEnumerable<TrainingModel>>> GetTrainings()
	{
		var client = await _httpClientFactoryWrapper.CreateClient("ApiGateway");
		var response = await client.GetAsync(_endpointProvider.TrainingManagementServiceEndpoint);

		return new()
		{
			StatusCode = response.StatusCode,
			Content = await response.GetContentAs<IEnumerable<TrainingModel>>()
		};
	}
}