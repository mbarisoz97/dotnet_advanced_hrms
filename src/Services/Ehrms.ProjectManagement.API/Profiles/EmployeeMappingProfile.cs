namespace Ehrms.ProjectManagement.API.Profiles;

internal class EmployeeMappingProfile : Profile
{
	public EmployeeMappingProfile()
	{
		CreateMap<Employee, ReadEmployeeDto>()
			.ForMember(dest => dest.Name, opt=> opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
	}
}