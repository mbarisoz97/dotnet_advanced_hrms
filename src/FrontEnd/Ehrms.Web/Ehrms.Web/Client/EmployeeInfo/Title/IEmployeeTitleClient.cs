using Ehrms.Web.Models.EmployeeInfo;

namespace Ehrms.Web.Client.EmployeeInfo.Title;

internal interface IEmployeeTitleClient
{
    Task<Response<IEnumerable<ReadEmployeeTitleModel>>> GetAllTitles();
}