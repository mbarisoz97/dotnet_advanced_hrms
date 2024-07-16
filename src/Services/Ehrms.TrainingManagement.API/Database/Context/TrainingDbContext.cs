using Ehrms.TrainingManagement.API.Database.Models;

namespace Ehrms.TrainingManagement.API.Database.Context;

public class TrainingDbContext : DbContext
{
    public TrainingDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Training> Trainings { get; set; }
    public DbSet<Employee> Employees { get; set; }
}