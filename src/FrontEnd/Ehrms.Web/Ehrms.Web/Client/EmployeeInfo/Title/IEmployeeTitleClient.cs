using Ehrms.Web.Models.EmployeeInfo;

namespace Ehrms.Web.Client.EmployeeInfo.Title;

internal interface IEmployeeTitleClient
{
    Task<Response<EmployeeTitleModel>> CreateTitle(EmployeeTitleModel employeeTitle);
    Task<Response<EmployeeTitleModel>> UpdateTitle(EmployeeTitleModel employeeTitle);
    Task<Response<IEnumerable<EmployeeTitleModel>>> GetAllTitles();
    Task<Response<EmployeeTitleModel>> GetTitleById(Guid Id);
}