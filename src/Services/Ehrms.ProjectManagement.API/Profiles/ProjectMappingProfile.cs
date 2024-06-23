namespace Ehrms.ProjectManagement.API.Profiles;

internal class ProjectMappingProfile : Profile
{
    public ProjectMappingProfile()
    {
        AddModelToDtoMappings();
        AddDtoToCommandMappings();
        AddCommandToModelMappings();
    }

    private void AddModelToDtoMappings()
    {
        CreateMap<Project, ReadProjectDto>();
    }

    private void AddCommandToModelMappings()
    {
        CreateMap<CreateProjectCommand, Project>();
        CreateMap<UpdateProjectCommand, Project>();
    }

    private void AddDtoToCommandMappings()
    {
        CreateMap<CreateProjectDto, CreateProjectCommand>();
        CreateMap<UpdateProjectDto, UpdateProjectCommand>();
    }
}