using Ehrms.Contracts.Employee;

namespace Ehrms.TrainingManagement.API.Profiles;

public class EmployeeMappingProfile : Profile
{
	public EmployeeMappingProfile()
	{
		CreateMap<Employee, ReadEmployeeDto>()
			.ForMember(dest => dest.FullName, opt => opt.MapFrom(x=> x.FirstName + " " + x.LastName));

		CreateMap<EmployeeCreatedEvent, Employee>()
			.ForMember(dest => dest.Skills, opt => opt.Ignore());

		CreateMap<EmployeeUpdatedEvent, Employee>()
            .ForMember(dest => dest.Skills, opt => opt.Ignore()); ;
	}
}