using System.Reflection;
using Ehrms.TrainingManagement.API.MessageQueue.StateMachine;

namespace Ehrms.TrainingManagement.API.Database.Context;

public class TrainingDbContext : DbContext
{
    public TrainingDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
    
    public DbSet<Training> Trainings { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Skill> Skills { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<TrainingRecommendationRequest> RecommendationRequests { get; set; }
    public DbSet<TrainingRecommendationResult> RecommendationResults { get; set; }
    public DbSet<TrainingRecommendationSagaData> RecommendationSagaData { get; set; }
}