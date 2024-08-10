using Ehrms.Contracts.Skill;

namespace Ehrms.TrainingManagement.API.Profiles;

public class SkillMappingProfile : Profile
{
    public SkillMappingProfile()
    {
		CreateMap<SkillCreatedEvent, Skill>();
		CreateMap<SkillUpdatedEvent, Skill>();
	}
}
