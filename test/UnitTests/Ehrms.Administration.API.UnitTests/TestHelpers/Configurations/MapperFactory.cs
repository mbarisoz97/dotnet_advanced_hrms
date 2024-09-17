using AutoMapper;
using Ehrms.Administration.API.Profiles;

namespace Ehrms.Administration.API.UnitTests.TestHelpers.Configurations;

internal static class MapperFactory
{
    internal static IMapper CreateWithExistingProfiles()
    {
        return new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.AddProfiles([
                new EmployeeMappingProfile(),
                new PaymentCategoryMappingProfiles(),
                new PaymentCategoryMappingProfiles(),
            ]);
        }));
    }
}