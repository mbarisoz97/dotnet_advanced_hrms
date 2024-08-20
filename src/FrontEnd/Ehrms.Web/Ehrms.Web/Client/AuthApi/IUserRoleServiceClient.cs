using Ehrms.Web.Models.User;

namespace Ehrms.Web.Client.AuthApi;

public interface IUserRoleServiceClient
{
    Task<Response<IEnumerable<UserRoleModel>>> GetAllUserRoles();
}
