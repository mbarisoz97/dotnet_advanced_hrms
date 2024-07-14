using System.ComponentModel.DataAnnotations;

namespace Ehrms.Web.Models;

public sealed class EmployeeModel
{
	public Guid Id { get; set; }

	[Required]
	[Length(2, 50)]
	public string? FirstName { get; set; }

	[Required]
	[Length(2, 50)]
	public string? LastName { get; set; }

	[Required]
	[DataType(DataType.Date)]
	public DateOnly? DateOfBirth { get; set; }

	[Required]
	[Length(2, 50)]
	public string? Qualification { get; set; }

	public override string ToString()
	{
		return $"{FirstName} {LastName}";
	}
}