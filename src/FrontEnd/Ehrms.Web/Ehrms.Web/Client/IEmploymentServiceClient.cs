namespace Ehrms.Web.Client;

public interface IEmploymentServiceClient
{
	Task<Response<IEnumerable<WorkerEmploymentModel>>> GetEmploymenHistoryByEmployeeId(Guid employeeId);
}