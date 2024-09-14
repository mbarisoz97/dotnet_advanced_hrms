namespace Ehrms.Web.Client.EmployeeApi.Info;

public interface IEmployeeServiceClient
{
    Task<Response<Guid>> DeleteEmployeeAsync(Guid id);
    Task<Response<EmployeeModel>> GetEmployeeAsync(Guid id);
    Task<Response<IEnumerable<EmployeeModel>>> GetEmployeesAsync();
    Task<Response<EmployeeModel>> CreateEmployeeAsync(EmployeeModel employee);
    Task<Response<EmployeeModel>> UpdateEmployeeAsync(EmployeeModel employee);
}