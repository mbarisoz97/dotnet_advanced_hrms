namespace Ehrms.Web.Client.ProjectApi.Employment;

public interface IEmploymentServiceClient
{
    Task<Response<IEnumerable<WorkerEmploymentModel>>> GetEmploymenHistoryByEmployeeId(Guid employeeId);
}