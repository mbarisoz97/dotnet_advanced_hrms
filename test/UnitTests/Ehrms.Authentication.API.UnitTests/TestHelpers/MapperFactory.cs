using AutoMapper;
using Ehrms.Authentication.API.Profiles;

namespace Ehrms.Authentication.API.UnitTests.TestHelpers;

internal static class MapperFactory
{
    internal static IMapper CreateWithExistingProfiles()
    {
        return new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.AddProfiles([
                new UserMappingProfile()
                ]);
        }));
    }
}
