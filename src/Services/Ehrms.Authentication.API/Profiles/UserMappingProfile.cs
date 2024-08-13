using Ehrms.Authentication.API.Dto;

namespace Ehrms.Authentication.API.Profiles;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<UpdateUserCommand, User>();
        CreateMap<User, ReadUserDto>();
    }
}