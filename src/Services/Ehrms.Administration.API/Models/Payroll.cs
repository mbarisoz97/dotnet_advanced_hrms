namespace Ehrms.Administration.API.Models;

public class Payroll : BaseEntity
{
	public DateOnly CreatedAt { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
	public PaymentCriteria PaymentCriteria { get; set; }
}
