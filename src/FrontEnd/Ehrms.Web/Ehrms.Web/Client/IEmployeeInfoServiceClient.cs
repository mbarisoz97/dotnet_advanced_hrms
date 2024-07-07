using Ehrms.Web.Models;

namespace Ehrms.Web.Client;

public interface IEmployeeInfoServiceClient
{
	Task CreateEmployee(Employee employee);
	Task<IEnumerable<Employee>> GetEmployees(int page, int pageSize = 20);
}