namespace Ehrms.TrainingManagement.API.Profiles;

public class ProjectMappingProfile : Profile
{
	public ProjectMappingProfile()
	{
		CreateMap<ProjectCreatedEvent, Project>()
			.ForMember(x => x.Employees, src => src.Ignore())
			.ForMember(x => x.RequiredSkills, src => src.Ignore());

		CreateMap<ProjectUpdatedEvent, Project>()
			.ForMember(x => x.Employees, src => src.Ignore())
			.ForMember(x => x.RequiredSkills, src => src.Ignore());

		CreateMap<ProjectDeletedEvent, Project>();
	}
}
