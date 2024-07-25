using Ehrms.Contracts.Employee;

namespace Ehrms.TrainingManagement.API.Profiles;

public class EmployeeMappingProfile : Profile
{
	public EmployeeMappingProfile()
	{
		CreateMap<EmployeeCreatedEvent, Employee>();
		CreateMap<EmployeeUpdatedEvent, Employee>();
	}
}
