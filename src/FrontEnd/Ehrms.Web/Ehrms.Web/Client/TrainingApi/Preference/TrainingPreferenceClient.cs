using Ehrms.Web.Routing;

namespace Ehrms.Web.Client.TrainingApi.Preference;

internal class TrainingPreferenceClient : ITrainingPreferenceClient
{
    private const string RecommendationPreferences = "RecommendationPreferences";

    private readonly IEndpointProvider _endpointProvider;
    private readonly IHttpClientFactoryWrapper _httpClientFactory;

    public TrainingPreferenceClient(IEndpointProvider endpointProvider, IHttpClientFactoryWrapper httpClientFactory)
    {
        _endpointProvider = endpointProvider;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<Response<TrainingPreferenceModel>> CreateTrainingPreferenceAsync(CreateTrainingPreferenceModel createTrainingPreference)
    {
        var client = await _httpClientFactory.CreateClient(HttpClients.BackendApiGateway);
        var response = await client.PutAsJsonAsync($"{_endpointProvider.TrainingRecommendationServiceEndpoint}/{RecommendationPreferences}", createTrainingPreference);

        return new()
        {
            StatusCode = response.StatusCode,
            Content = await response.GetContentAs<TrainingPreferenceModel>()
        };
    }

    public async Task<Response<IEnumerable<TrainingPreferenceModel>>> GetTrainingPreferenceAsync()
    {
        var client = await _httpClientFactory.CreateClient(HttpClients.BackendApiGateway);
        var response = await client.GetAsync($"{_endpointProvider.TrainingRecommendationServiceEndpoint}/{RecommendationPreferences}");

        return new()
        {
            StatusCode = response.StatusCode,
            Content = await response.GetContentAs<IEnumerable<TrainingPreferenceModel>>()
        };
    }

    public async Task<Response<TrainingPreferenceModel>> GetTrainingPreferenceByIdAsync(Guid id)
    {
        var client = await _httpClientFactory.CreateClient(HttpClients.BackendApiGateway);
        var response = await client.GetAsync($"{_endpointProvider.TrainingRecommendationServiceEndpoint}/{RecommendationPreferences}/{id}");

        return new()
        {
            StatusCode = response.StatusCode,
            Content = await response.GetContentAs<TrainingPreferenceModel>()
        };
    }

    public async Task<Response<Guid>> DeleteTrainingPreferenceByIdAsync(Guid id)
    {
        var client = await _httpClientFactory.CreateClient(HttpClients.BackendApiGateway);
        var response = await client.DeleteAsync($"{_endpointProvider.TrainingRecommendationServiceEndpoint}/{RecommendationPreferences}/{id}");

        return new()
        {
            StatusCode = response.StatusCode,
            Content = await response.GetContentAs<Guid>()
        };
    }

    public async Task<Response<TrainingPreferenceModel>> UpdateTrainingPreferenceAsync(UpdateTrainingPreferenceModel updateTrainingPreferenceModel)
    {
        var client = await _httpClientFactory.CreateClient(HttpClients.BackendApiGateway);
        var response = await client.PostAsJsonAsync($"{_endpointProvider.TrainingRecommendationServiceEndpoint}/{RecommendationPreferences}", updateTrainingPreferenceModel);

        return new()
        {
            StatusCode = response.StatusCode,
            Content = await response.GetContentAs<TrainingPreferenceModel>()
        };
    }
}