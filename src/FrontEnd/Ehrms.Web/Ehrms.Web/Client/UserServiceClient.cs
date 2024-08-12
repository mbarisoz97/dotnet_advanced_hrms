namespace Ehrms.Web.Client;

internal class UserServiceClient : IUserServiceClient
{
    private readonly IEndpointProvider _endpointProvider;
    private readonly IHttpClientFactoryWrapper _httpClientFactoryWrapper;

    public UserServiceClient(IEndpointProvider endpointProvider, IHttpClientFactoryWrapper httpClientFactoryWrapper)
    {
        _endpointProvider = endpointProvider;
        _httpClientFactoryWrapper = httpClientFactoryWrapper;
    }

    public async Task<Response<IEnumerable<ReadUserModel>>> GetUsersAsync()
    {
        var client = await _httpClientFactoryWrapper.CreateClient("ApiGateway");
        var response = await client.GetAsync(_endpointProvider.UserEndpoint);

        return new()
        {
            StatusCode = response.StatusCode,
            Content = await response.GetContentAs<IEnumerable<ReadUserModel>>()
        };
    }
}