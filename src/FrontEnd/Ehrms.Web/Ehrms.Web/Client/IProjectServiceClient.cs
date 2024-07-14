using Ehrms.Web.Models;

namespace Ehrms.Web.Client;

public interface IProjectServiceClient
{
	Task<Response<ProjectModel>> CreateProjectAsync(ProjectModel project);
	Task<Response<ProjectModel>> GetProjectAsync(Guid id);
    Task<Response<Guid>> DeleteProjectAsync(Guid id);
    Task<Response<IEnumerable<ProjectModel>>> GetProjectsAsync();
	Task<Response<ProjectModel>> UpdateProjectAsync(ProjectModel project);
}