using Ehrms.Contracts.Skill;

namespace Ehrms.ProjectManagement.API.Profiles;

internal class SkillMappingProfile : Profile
{
	public SkillMappingProfile()
	{
		CreateMap<SkillCreatedEvent, Skill>();
		CreateMap<SkillUpdatedEvent, Skill>();
	}
}