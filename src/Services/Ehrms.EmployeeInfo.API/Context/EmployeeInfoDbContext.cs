using Microsoft.EntityFrameworkCore;

namespace Ehrms.EmployeeInfo.API.Context;

public class EmployeeInfoDbContext : DbContext
{
    public EmployeeInfoDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Skill> Skills { get; set; }
}