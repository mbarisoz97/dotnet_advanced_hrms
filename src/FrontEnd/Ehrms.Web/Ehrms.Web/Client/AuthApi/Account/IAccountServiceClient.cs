namespace Ehrms.Web.Client.AuthApi.Account;

internal interface IAccountServiceClient
{
    Task<Response<LoginResponseModel>> Login(LoginRequestModel? loginRequest);
    Task<Response<LoginResponseModel>> RefreshSession(LoginRefreshModel? loginRefreshRequest);
}