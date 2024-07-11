using Ehrms.Web.Models;

namespace Ehrms.Web.Client;

public interface IEmployeeServiceClient
{
	Task CreateEmployeeAsync(Employee employee);
	Task<Employee> UpdateEmployeeAsync(Employee employee);
	Task DeleteEmployeeAsync(Guid id);
	Task<Employee> GetEmployeeAsync(Guid id);
	Task<IEnumerable<Employee>> GetEmployeesAsync(int pageNumber, int pageSize = 20);
}