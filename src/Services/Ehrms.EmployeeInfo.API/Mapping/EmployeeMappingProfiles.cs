using Ehrms.EmployeeInfo.API.Database.Models;

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
            .ForMember(dest => dest.Skills, opt => opt.Ignore())
            .ForMember(dest => dest.Title, opt => opt.Ignore());

        CreateMap<CreateEmployeeCommand, Employee>()
            .ForMember(dest => dest.Skills, opt => opt.Ignore())
            .ForMember(dest => dest.Title, opt => opt.Ignore());
    }

    private void AddEntityToDtoMappings()
    {
        CreateMap<Employee, ReadTitleDto>()
            .ForMember(dest => dest.Skills, opt => opt.MapFrom(src => src.Skills.Select(x => x.Id)));
    }

    private void AddModelToMessageQueueEventMappings()
    {
        CreateMap<Employee, EmployeeCreatedEvent>()
            .ForMember(dest => dest.Skills, opt => opt.MapFrom(x => x.Skills.Select(y => y.Id).ToList()));
        CreateMap<Employee, EmployeeUpdatedEvent>()
             .ForMember(dest => dest.Skills, opt => opt.MapFrom(x => x.Skills.Select(y => y.Id).ToList()));
    }
}