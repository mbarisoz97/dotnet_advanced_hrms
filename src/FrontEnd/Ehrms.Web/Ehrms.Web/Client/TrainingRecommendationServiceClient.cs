using Ehrms.Web.Routing;

namespace Ehrms.Web.Client;

internal sealed class TrainingRecommendationServiceClient : ITrainingRecommendationServiceClient
{
    private readonly IEndpointProvider _endpointProvider;
    private readonly IHttpClientFactoryWrapper _httpClientFactoryWrapper;

    public TrainingRecommendationServiceClient(IEndpointProvider endpointProvider, IHttpClientFactoryWrapper httpClientFactoryWrapper)
    {
        _endpointProvider = endpointProvider;
        _httpClientFactoryWrapper = httpClientFactoryWrapper;
    }
    
    public async Task<Response<ReadTrainingRecommendationResultModel>> GetRecommendationResult(Guid id)
    {
        var client = await _httpClientFactoryWrapper.CreateClient("ApiGateway");
        var response = await client.GetAsync($"{_endpointProvider.TrainingRecommendationServiceEndpoint}/RecommendationResult/{id}");

        return new()
        {
            StatusCode = response.StatusCode,
            Content = await response.GetContentAs<ReadTrainingRecommendationResultModel>()
        };
    }

    public async Task<Response<IEnumerable<ReadTrainingRecommendationResultModel>>> GetRecommendationResults(Guid id)
    {
        var client = await _httpClientFactoryWrapper.CreateClient("ApiGateway");
        var response = await client.GetAsync($"{_endpointProvider.TrainingRecommendationServiceEndpoint}/RecommendationResults/{id}");

        return new()
        {
            StatusCode = response.StatusCode,
            Content = await response.GetContentAs<IEnumerable<ReadTrainingRecommendationResultModel>>()
        };
    }

    public async Task<Response<Guid>> CreateTrainingRecommendationRequest(CreateTrainingRecommendationRequestModel createTrainingRecommendationRequest)
    {
        var client = await _httpClientFactoryWrapper.CreateClient("ApiGateway");
        var response = await client.PostAsJsonAsync($"{_endpointProvider.TrainingRecommendationServiceEndpoint}/Recommendation", createTrainingRecommendationRequest);

        return new()
        {
            StatusCode = response.StatusCode,
            Content = await response.GetContentAs<Guid>()
        };
    }

    public async Task<Response<IEnumerable<ReadTrainingRecommendationRequestModel>>> GetTrainingRecommendationRequests()
    {
        var client = await _httpClientFactoryWrapper.CreateClient("ApiGateway");
        var response = await client.GetAsync($"{_endpointProvider.TrainingRecommendationServiceEndpoint}/RecommendationRequests");

        return new()
        {
            StatusCode = response.StatusCode,
            Content = await response.GetContentAs<IEnumerable<ReadTrainingRecommendationRequestModel>>()
        };
    }

    public async Task<Response<ReadTrainingRecommendationRequestModel>> GetTrainingRecommendationRequest(Guid id)
    {
        var client = await _httpClientFactoryWrapper.CreateClient("ApiGateway");
        var response = await client.GetAsync($"{_endpointProvider.TrainingRecommendationServiceEndpoint}/Recommendation/{id}");

        return new()
        {
            StatusCode = response.StatusCode,
            Content = await response.GetContentAs<ReadTrainingRecommendationRequestModel>()
        };
    }

    public async Task<Response<Guid>> DeleteTrainingRecommendationRequest(Guid id)
    {
        var client = await _httpClientFactoryWrapper.CreateClient("ApiGateway");
        var response = await client.DeleteAsync($"{_endpointProvider.TrainingRecommendationServiceEndpoint}/Recommendation/{id}");

        return new()
        {
            StatusCode = response.StatusCode,
        };
    }

}