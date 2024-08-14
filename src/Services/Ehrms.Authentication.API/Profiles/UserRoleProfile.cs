using Ehrms.Authentication.API.Dto.Role;

namespace Ehrms.Authentication.API.Profiles;

public class UserRoleProfile : Profile
{
    public UserRoleProfile()
    {
        CreateMap<Role, ReadUserRoleDto>();
    }
}