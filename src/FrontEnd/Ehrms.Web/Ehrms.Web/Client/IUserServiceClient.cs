namespace Ehrms.Web.Client;

internal interface IUserServiceClient
{
    Task<Response<IEnumerable<ReadUserModel>>> GetUsersAsync();
}