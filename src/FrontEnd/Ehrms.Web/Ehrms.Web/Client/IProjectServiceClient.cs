using Ehrms.Web.Models;

namespace Ehrms.Web.Client;

public interface IProjectServiceClient
{
    Task<Response<IEnumerable<ProjectModel>>> GetProjectsAsync();
}
