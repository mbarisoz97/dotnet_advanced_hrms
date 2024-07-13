namespace Ehrms.ProjectManagement.API.Context;

public class ProjectDbContext : DbContext
{
    public ProjectDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Project> Projects { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Employment> Employments { get; set; }
}