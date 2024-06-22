using Ehrms.EmployeeInfo.API.Handlers;

namespace Ehrms.EmployeeInfo.API.Mapping;

public class EmployeeMappingProfiles : Profile
{
    public EmployeeMappingProfiles()
    {
        AddDtoToCommandMappings();
        AddEntityToDtoMappings();
        AddCommandToEntityMappings();
    }

    private void AddCommandToEntityMappings()
    {
        CreateMap<UpdateEmployeeCommand, Employee>();
        CreateMap<CreateEmployeeCommand, Employee>();
    }

    private void AddEntityToDtoMappings()
    {
        CreateMap<Employee, ReadEmployeeDto>()
            .ForMember(dest => dest.Skills,
                opt => opt.MapFrom(
                    src => src.Skills.Select(x=>x.Id)));
    }

    private void AddDtoToCommandMappings()
    {
        CreateMap<CreateEmployeeDto, CreateEmployeeCommand>();
        CreateMap<UpdateEmployeeDto, UpdateEmployeeCommand>();
    }
}