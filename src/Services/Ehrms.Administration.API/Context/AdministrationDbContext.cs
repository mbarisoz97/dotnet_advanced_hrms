namespace Ehrms.Administration.API.Context;

public class AdministrationDbContext : DbContext
{
	public AdministrationDbContext(DbContextOptions options) : base(options)
	{
	}

	public DbSet<Payroll> Payrolls { get; set; }
	public DbSet<Employee> Employees { get; set; }
	public DbSet<PaymentCriteria> PaymentCriteria { get; set; }
	public DbSet<PaymentCategory> PaymentCategories { get; set; }
}