using Ehrms.Authentication.API.Dto.User;

namespace Ehrms.Authentication.API.Profiles;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        AddCommandToModelMappings();
        AddModelToDtoMappings();
    }

    private void AddModelToDtoMappings()
    {
        CreateMap<User, UserUpdateResponseDto>();
        CreateMap<User, RegisterUserResponseDto>();
        CreateMap<User, ReadUserDto>()
            .ForMember(dest => dest.Roles, opt =>
                opt.MapFrom(src => src.UserRoles.Select(r => r.Role!.Name)));
    }

    private void AddCommandToModelMappings()
    {
        CreateMap<UpdateUserCommand, User>()
            .ForMember(dest => dest.UserRoles, opt => opt.Ignore());

        CreateMap<RegisterUserCommand, User>()
             .ForMember(dest => dest.UserRoles, opt => opt.Ignore());
    }
}