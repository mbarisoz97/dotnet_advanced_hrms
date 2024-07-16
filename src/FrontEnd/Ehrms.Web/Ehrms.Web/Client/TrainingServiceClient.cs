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