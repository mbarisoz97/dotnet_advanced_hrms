using Ehrms.Web.Models;

namespace Ehrms.Web.Client;

public interface IEmployeeInfoServiceClient
{
	Task<IEnumerable<Employee>> GetEmployees();
}