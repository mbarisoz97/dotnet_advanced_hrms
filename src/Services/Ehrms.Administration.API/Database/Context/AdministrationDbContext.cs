using Ehrms.Administration.API.Database.Models;
using System.Reflection;

namespace Ehrms.Administration.API.Database.Context;

public class AdministrationDbContext : DbContext
{
    public AdministrationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<PaymentCriteria> PaymentCriteria { get; set; }
    public DbSet<PaymentCategory> PaymentCategories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}