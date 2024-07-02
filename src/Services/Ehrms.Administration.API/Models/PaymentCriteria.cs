namespace Ehrms.Administration.API.Models;

public class PaymentCriteria : BaseEntity
{
	public DateOnly StartedAt { get; set; }
	public DateOnly? ExpiredAt { get; set; }
	public Employee Employee { get; set; } 
}

