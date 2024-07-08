using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Ehrms.Authentication.API.Controllers;

public class ApplicationUserDbContext : IdentityDbContext<User, Role, Guid>
{
	public ApplicationUserDbContext(DbContextOptions options) : base(options)
	{
	}

	public DbSet<User> Users { get; set; }
}