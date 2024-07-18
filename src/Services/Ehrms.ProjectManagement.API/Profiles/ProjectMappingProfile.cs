﻿namespace Ehrms.ProjectManagement.API.Profiles;

internal class ProjectMappingProfile : Profile
{
	public ProjectMappingProfile()
	{
		AddModelToDtoMappings();
		AddCommandToModelMappings();
		AddMessageQueueEventToModelMappings();
	}

	private void AddModelToDtoMappings()
	{
		CreateMap<Project, ReadProjectDto>()
			.ForMember(dest => dest.Employees,
				opt => opt.MapFrom(
					src => src.Employments
					.Where(x => x.EndedAt == null)
					.Select(x => x.EmployeeId)))
			
			.ForMember(dest => dest.RequiredSkills,
				opt => opt.MapFrom(
					src=> src.RequiredProjectSkills.Select(x=> x.Id)));
	}

	private void AddCommandToModelMappings()
	{
		CreateMap<CreateProjectCommand, Project>();
		CreateMap<UpdateProjectCommand, Project>();
	}

	private void AddMessageQueueEventToModelMappings()
	{
		CreateMap<EmployeeCreatedEvent, Employee>();
		CreateMap<EmployeeUpdatedEvent, Employee>();
	}
}