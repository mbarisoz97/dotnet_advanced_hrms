using Ehrms.Web.Models.User;

namespace Ehrms.Web.Client.AuthService;

public interface IUserRoleServiceClient
{
    Task<Response<IEnumerable<UserRoleModel>>> GetAllUserRoles();
}
