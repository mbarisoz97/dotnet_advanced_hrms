using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Ehrms.Authentication.API.Database.Models;

namespace Ehrms.Authentication.API.Database.Context;

public class ApplicationUserDbContext : IdentityDbContext<User, Role, Guid>
{
	public ApplicationUserDbContext(DbContextOptions options) : base(options)
	{
	}

	public DbSet<User> Users { get; set; }
}