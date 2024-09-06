using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Reflection;

namespace Ehrms.Authentication.API.Database.Context;

public class ApplicationUserDbContext : IdentityDbContext<User, Role, Guid>
{
    public ApplicationUserDbContext(DbContextOptions options) : base(options)
    {
    }

    public override DbSet<User> Users { get; set; }
    public override DbSet<Role> Roles { get; set; }
    public new DbSet<UserRole> UserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}