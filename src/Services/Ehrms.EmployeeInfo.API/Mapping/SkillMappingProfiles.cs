namespace Ehrms.EmployeeInfo.API.Mapping;

public class SkillMappingProfiles : Profile
{
    public SkillMappingProfiles()
    {
        AddDtoToCommandMappings();
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

    private void AddDtoToCommandMappings()
    {
        CreateMap<CreateSkillDto, CreateSkillCommand>();
        CreateMap<UpdateSkillDto, UpdateSkillCommand>();
    }
}
