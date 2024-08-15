using Microsoft.AspNetCore.Identity;

namespace Ehrms.Authentication.API.Database.Models;

public class Role : IdentityRole<Guid>
{
    public Role() : this("")
    {
    }

    public Role(string roleName) : base(roleName)
    {
    }

    public ICollection<UserRole> UserRoles { get; set; } = [];
}