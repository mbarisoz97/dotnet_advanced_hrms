using System.Reflection;

namespace Ehrms.ProjectManagement.API.Database.Context;

public class ProjectDbContext : DbContext
{
	public ProjectDbContext(DbContextOptions options) : base(options)
	{
	}

	public DbSet<Project> Projects { get; set; }
	public DbSet<Employee> Employees { get; set; }
	public DbSet<Employment> Employments { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		base.OnModelCreating(modelBuilder);
	}
}