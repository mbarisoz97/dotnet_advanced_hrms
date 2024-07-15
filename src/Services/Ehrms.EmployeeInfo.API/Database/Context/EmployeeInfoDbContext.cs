using System.Reflection;
using Ehrms.EmployeeInfo.API.Database.Models;
using Elastic.CommonSchema;
using Microsoft.AspNetCore.Identity;

namespace Ehrms.EmployeeInfo.API.Database.Context;

public class EmployeeInfoDbContext : DbContext
{
    public EmployeeInfoDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Skill> Skills { get; set; }


	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		base.OnModelCreating(modelBuilder);
	}
}