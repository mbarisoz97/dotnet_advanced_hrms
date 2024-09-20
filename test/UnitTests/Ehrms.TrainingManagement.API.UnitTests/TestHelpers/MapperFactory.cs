using Ehrms.TrainingManagement.API.Profiles;

namespace Ehrms.TrainingManagement.API.UnitTests.TestHelpers;

internal static class MapperFactory
{
	internal static IMapper CreateWithExistingProfiles()
	{
		return new Mapper(new MapperConfiguration(cfg =>
		{
			cfg.AddProfiles([
				new TrainingMappingProfile(),
				new SkillMappingProfile(),
				new ProjectMappingProfile(),
				new EmployeeMappingProfile(),
				new TitleMappingProfile()
			]);
		}));
	}
}