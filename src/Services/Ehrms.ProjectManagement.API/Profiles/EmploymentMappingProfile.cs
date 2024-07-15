namespace Ehrms.ProjectManagement.API.Profiles;

internal class EmploymentMappingProfile : Profile
{
	public EmploymentMappingProfile()
	{
		CreateMap<Employment, EmploymentDto>()
			.ForMember(x => x.EmployeeName, opt => opt.MapFrom(src => src.Employee.FirstName + " " + src.Employee.LastName));
	}
}