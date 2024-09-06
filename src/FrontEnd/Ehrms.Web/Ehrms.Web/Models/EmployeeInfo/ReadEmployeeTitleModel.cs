using System.ComponentModel.DataAnnotations;

namespace Ehrms.Web.Models.EmployeeInfo;

public sealed class EmployeeTitleModel
{
    public Guid Id { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
    public string TitleName { get; set; } = string.Empty;
}