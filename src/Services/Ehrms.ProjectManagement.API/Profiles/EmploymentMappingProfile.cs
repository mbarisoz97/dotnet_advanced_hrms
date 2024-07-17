namespace Ehrms.ProjectManagement.API.Profiles;

internal class EmploymentMappingProfile : Profile
{
	public EmploymentMappingProfile()
	{
		CreateMap<Employment, ProjectEmploymentDto>()
			.ForMember(x => x.EmployeeName, opt => opt.MapFrom(src => src.Employee.FirstName + " " + src.Employee.LastName));

		CreateMap<Employment, WorkerEmploymentDto>()
			.ForMember(x => x.ProjectName, opt => opt.MapFrom(src => src.Project.Name));
	}
}