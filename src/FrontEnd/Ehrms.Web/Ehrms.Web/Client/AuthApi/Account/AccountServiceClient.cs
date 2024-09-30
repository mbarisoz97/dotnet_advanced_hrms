using Ehrms.Web.Models.User;
using Ehrms.Web.Routing;

namespace Ehrms.Web.Client.AuthApi.Account;

internal sealed class AccountServiceClient : IAccountServiceClient
{
    private readonly IEndpointProvider _endpointProvider;
    private readonly IHttpClientFactoryWrapper _clientFactoryWrapper;

    public AccountServiceClient(IEndpointProvider endpointProvider, IHttpClientFactoryWrapper clientFactoryWrapper)
    {
        _endpointProvider = endpointProvider;
        _clientFactoryWrapper = clientFactoryWrapper;
    }

    public async Task<Response<LoginResponseModel>> Login(LoginRequestModel? loginRequest)
    {
        var client = await _clientFactoryWrapper.CreateClient(HttpClients.BackendApiGateway);
        var response = await client.PostAsJsonAsync(_endpointProvider.AutheticationEndpoint, loginRequest);
        
        return new()
        {
            StatusCode = response.StatusCode,
            Content =  await response.GetContentAs<LoginResponseModel>()
        };
    }

    public async Task<Response<LoginResponseModel>> RefreshSession(LoginRefreshModel? loginRefreshRequest)
    {
        var client = await _clientFactoryWrapper.CreateClient(HttpClients.BackendApiGateway);
        var response = await client.PostAsJsonAsync(_endpointProvider.RefreshEndpoint, loginRefreshRequest);
        
        return new()
        {
            StatusCode = response.StatusCode,
            Content =  await response.GetContentAs<LoginResponseModel>()
        };
    }

    public async Task<Response<ReadUserModel>> ResetPassword(PasswordResetModel? resetPasswordRequest)
    {
        var client = await _clientFactoryWrapper.CreateClient(HttpClients.BackendApiGateway);
        var response = await client.PostAsJsonAsync($"{_endpointProvider.UserEndpoint}/Reset", resetPasswordRequest);
        
        return new()
        {
            StatusCode = response.StatusCode,
            Content =  await response.GetContentAs<ReadUserModel>()
        };
    }
}