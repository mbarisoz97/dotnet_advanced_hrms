namespace Ehrms.EmployeeInfo.API.Mapping;

public class EmployeeMappingProfiles : Profile
{
    public EmployeeMappingProfiles()
    {
        AddEntityToDtoMappings();
        AddCommandToEntityMappings();
        AddModelToMessageQueueEventMappings();
    }

    private void AddCommandToEntityMappings()
    {
        CreateMap<UpdateEmployeeCommand, Employee>()
            .ForMember(dest => dest.Skills, opt => opt.Ignore());

        CreateMap<CreateEmployeeCommand, Employee>()
            .ForMember(dest => dest.Skills, opt => opt.Ignore());
    }

    private void AddEntityToDtoMappings()
    {
        CreateMap<Employee, ReadEmployeeDto>()
            .ForMember(dest => dest.Skills,
                opt => opt.MapFrom(
                    src => src.Skills.Select(x=>x.Id)));
    }

    private void AddModelToMessageQueueEventMappings()
    {
        CreateMap<Employee, EmployeeCreatedEvent>();
		CreateMap<Employee, EmployeeUpdatedEvent>();
	}
}