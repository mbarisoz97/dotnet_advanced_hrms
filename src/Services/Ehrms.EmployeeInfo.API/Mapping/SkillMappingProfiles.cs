namespace Ehrms.EmployeeInfo.API.Mapping;

public class SkillMappingProfiles : Profile
{
    public SkillMappingProfiles()
    {
        AddEntityToDtoMappings();
        AddCommandToEntityMappings();
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
