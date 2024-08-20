using Ehrms.Web.Models.User;

namespace Ehrms.Web.Client.AuthApi;

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

    public async Task<Response<ReadUserModel>> GetUserByIdAsync(Guid id)
    {
        var client = await _httpClientFactoryWrapper.CreateClient("ApiGateway");
        var response = await client.GetAsync($"{_endpointProvider.UserEndpoint}/{id}");

        return new()
        {
            StatusCode = response.StatusCode,
            Content = await response.GetContentAs<ReadUserModel>()
        };
    }

    public async Task<Response<ReadUserModel>> UpdateUserAsync(ReadUserModel model)
    {
        var client = await _httpClientFactoryWrapper.CreateClient("ApiGateway");
        var response = await client.PostAsJsonAsync(_endpointProvider.UserEndpoint + "/Update", model);

        return new()
        {
            StatusCode = response.StatusCode,
            Content = await response.GetContentAs<ReadUserModel>()
        };
    }

    public async Task<Response<ReadUserModel>> RegisterUserAsync(RegisterUserModel model)
    {
        var client = await _httpClientFactoryWrapper.CreateClient("ApiGateway");
        var response = await client.PutAsJsonAsync(_endpointProvider.UserEndpoint + "/Register", model);

        return new()
        {
            StatusCode = response.StatusCode,
            Content = await response.GetContentAs<ReadUserModel>()
        };
    }
}