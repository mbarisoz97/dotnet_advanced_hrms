namespace Ehrms.ProjectManagement.API.Profiles;

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
            .ForMember(dest => dest.EmployeeIdCollection,
                opt => opt.MapFrom(
                    src => src.Employments.Select(x => x.Employee!.Id)));
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