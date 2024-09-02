namespace Ehrms.EmployeeInfo.API.UnitTests.TestHelpers;

internal static class MapperFactory
{
    internal static IMapper CreateWithExistingProfiles()
    {
        return new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.AddProfiles([
                new EmployeeMappingProfiles(),
                new SkillMappingProfiles(),
                new TitleMappingProfile(),
            ]);
        }));
    }
}