using Ehrms.Authentication.API.Handlers.Auth.Commands;
using Ehrms.Shared;

namespace Ehrms.Authentication.API.Profiles;

public class AuthMappingProfile : Profile
{
    public AuthMappingProfile()
    {
        CreateMap<AuthenticateUserCommand, GenerateJwtRequest>();
        CreateMap<RefreshAuthenticationCommand, GenerateJwtRequest>();
    }
}