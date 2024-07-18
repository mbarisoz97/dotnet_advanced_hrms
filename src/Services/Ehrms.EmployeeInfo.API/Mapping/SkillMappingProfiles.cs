using Ehrms.Contracts.Skill;
using Ehrms.EmployeeInfo.API.Database.Models;

namespace Ehrms.EmployeeInfo.API.Mapping;

public class SkillMappingProfiles : Profile
{
    public SkillMappingProfiles()
	{
		AddEntityToDtoMappings();
		AddCommandToEntityMappings();
		AddEntityToMessageQueueEventMappings();
	}

	private void AddEntityToMessageQueueEventMappings()
	{
		CreateMap<Skill, SkillCreatedEvent>();
		CreateMap<Skill, SkillUpdatedEvent>();
	}

	private void AddCommandToEntityMappings()
    {
        CreateMap<UpdateSkillCommand, Skill>();
        CreateMap<CreateSkillCommand, Skill>();
    }

    private void AddEntityToDtoMappings()
    {
        CreateMap<Skill, ReadSkillDto>();
    }
}
