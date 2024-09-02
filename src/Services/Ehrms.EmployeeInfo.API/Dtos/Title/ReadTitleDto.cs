namespace Ehrms.EmployeeInfo.API.Dtos.Title;

public sealed class ReadTitleDto
{
    public Guid Id { get; set; }
    public string TitleName { get; set; } = string.Empty;
}