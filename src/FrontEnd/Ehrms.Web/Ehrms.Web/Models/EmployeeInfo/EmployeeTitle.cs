namespace Ehrms.Web.Models.EmployeeInfo;

public sealed class ReadEmployeeTitleModel
{
    public Guid Id { get; set; }
    public string TitleName { get; set; } = string.Empty;
}