using Ehrms.TrainingManagement.API.Models;

namespace Ehrms.TrainingManagement.API.Context;

public class TrainingDbContext : DbContext
{
	public TrainingDbContext(DbContextOptions options) : base(options)
	{
	}

	public DbSet<Training> Trainings { get; set; }
	public DbSet<Employee> Employees { get; set; }
}