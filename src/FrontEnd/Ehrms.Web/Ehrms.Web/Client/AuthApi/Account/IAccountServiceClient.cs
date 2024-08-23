using Ehrms.Web.Models.User;
using Microsoft.AspNetCore.Identity.Data;

namespace Ehrms.Web.Client.AuthApi.Account;

internal interface IAccountServiceClient
{
    Task<Response<LoginResponseModel>> Login(LoginRequestModel? loginRequest);
    Task<Response<LoginResponseModel>> RefreshSession(LoginRefreshModel? loginRefreshRequest);
    Task<Response<ReadUserModel>> ResetPassword(PasswordResetModel? resetPasswordRequest);
}