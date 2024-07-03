namespace Ehrms.Administration.API.Dto.Payment;

public sealed class ReadPaymentDto
{
	public Guid Id { get; set; }
	public decimal Amount { get; set; }
	public DateTime CreatedAt { get; set; }
	public Guid EmployeeId { get; set; }
	public Guid PaymentCategoryId { get; set; }
}