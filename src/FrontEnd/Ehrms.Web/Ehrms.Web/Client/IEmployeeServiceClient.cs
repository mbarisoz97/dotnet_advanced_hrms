using Ehrms.Web.Models;

namespace Ehrms.Web.Client;

public interface IEmployeeServiceClient
{
    Task CreateEmployeeAsync(EmployeeModel employee);
    Task<EmployeeModel> UpdateEmployeeAsync(EmployeeModel employee);
    Task DeleteEmployeeAsync(Guid id);
    Task<EmployeeModel> GetEmployeeAsync(Guid id);
    Task<Response<IEnumerable<EmployeeModel>>> GetEmployeesAsync();
}
