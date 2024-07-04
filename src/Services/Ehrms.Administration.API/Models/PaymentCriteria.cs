namespace Ehrms.Administration.API.Models;

public class PaymentCriteria : BaseEntity
{
	public decimal Amount { get; set; }
	public DateTime CreatedAt { get; set; }	
	public Employee Employee { get; set; } 
	public PaymentCategory PaymentCategory { get; set; }	
}