using Ehrms.Authentication.API.Dto;

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
                opt.MapFrom(src => src.Roles.Select(r => r.Name)));
    }

    private void AddCommandToModelMappings()
    {
        CreateMap<UpdateUserCommand, User>()
            .ForMember(dest => dest.Roles, opt => opt.Ignore());
        
        CreateMap<RegisterUserCommand, User>()
             .ForMember(dest => dest.Roles, opt => opt.Ignore());
    }
}