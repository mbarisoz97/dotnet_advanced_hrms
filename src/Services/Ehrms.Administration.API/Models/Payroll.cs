namespace Ehrms.Administration.API.Models;

public class Payroll : BaseEntity
{
	public DateOnly CreatedAt { get; set; }
	public PaymentCriteria PaymentCriteria { get; set; }
}