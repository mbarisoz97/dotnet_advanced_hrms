using Ehrms.Web.Models;

namespace Ehrms.Web.Client;

public interface IProjectServiceClient
{
	Task<Response<ProjectModel>> CreateProjectAsync(ProjectModel projectModel);
	Task<Response<ProjectModel>> GetProjectAsync(Guid id);
	Task<Response<IEnumerable<ProjectModel>>> GetProjectsAsync();
}