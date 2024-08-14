using Microsoft.AspNetCore.Identity;

namespace Ehrms.Authentication.API.Database.Models;

public sealed class Role : IdentityRole<Guid>
{
    public Role()
    {
    }

    public Role(string roleName) : base(roleName)
    {
    }

    public ICollection<User> Users { get; set; } = [];
}
