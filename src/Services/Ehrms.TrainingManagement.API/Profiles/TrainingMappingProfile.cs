using Ehrms.Contracts.Employee;

namespace Ehrms.TrainingManagement.API.Profiles;

public class TrainingMappingProfile : Profile
{
	public TrainingMappingProfile()
	{
		AddModelToDtoMappings();
		AddEventToModelMappings();
		AddCommandToModelMappings();
	}

	private void AddCommandToModelMappings()
	{
		CreateMap<CreateTrainingCommand, Training>()
			.ForMember(dest => dest.Participants, opt => opt.Ignore());

		CreateMap<UpdateTrainingCommand, Training>()
			.ForMember(dest => dest.Participants, opt => opt.Ignore());
	}

	private void AddModelToDtoMappings()
	{
		CreateMap<Training, ReadTrainingDto>()
			.ForMember(dest => dest.Participants,
				opt => opt.MapFrom(src => src.Participants.Select(x => x.Id)));
	}

	private void AddEventToModelMappings()
	{
		CreateMap<EmployeeCreatedEvent, Employee>();
		CreateMap<EmployeeUpdatedEvent, Employee>();

		CreateMap<ProjectCreatedEvent, Project>()
			.ForMember(x=>x.Employees, src => src.Ignore())
			.ForMember(x=>x.RequiredSkills, src => src.Ignore());

		CreateMap<ProjectUpdatedEvent, Project>()
			.ForMember(x => x.Employees, src => src.Ignore())
			.ForMember(x => x.RequiredSkills, src => src.Ignore());
		
		CreateMap<ProjectDeletedEvent, Project>();
	}
}