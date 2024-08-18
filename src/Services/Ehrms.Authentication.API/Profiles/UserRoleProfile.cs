using Ehrms.Authentication.API.Dto.Role;

namespace Ehrms.Authentication.API.Profiles;

public class UserRoleMappingProfile : Profile
{
    public UserRoleMappingProfile()
    {
        CreateMap<Role, ReadUserRoleDto>();
    }
}