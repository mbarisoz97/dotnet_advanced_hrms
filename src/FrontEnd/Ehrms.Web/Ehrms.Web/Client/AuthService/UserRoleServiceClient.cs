using Ehrms.Web.Models.User;

namespace Ehrms.Web.Client.AuthService;

internal class UserRoleServiceClient : IUserRoleServiceClient
{
    private readonly IEndpointProvider _endpointProvider;
    private readonly IHttpClientFactoryWrapper _httpClientFactoryWrapper;

    public UserRoleServiceClient(IEndpointProvider endpointProvider, IHttpClientFactoryWrapper httpClientFactoryWrapper)
    {
        _endpointProvider = endpointProvider;
        _httpClientFactoryWrapper = httpClientFactoryWrapper;
    }

    public async Task<Response<IEnumerable<UserRoleModel>>> GetAllUserRoles()
    {
        var client = await _httpClientFactoryWrapper.CreateClient("ApiGateway");
        var response = await client.GetAsync(_endpointProvider.UserRoleEndpoint);

        return new()
        {
            StatusCode = response.StatusCode,
            Content = await response.GetContentAs<IEnumerable<UserRoleModel>>()
        };
    }
}