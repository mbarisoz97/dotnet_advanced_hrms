using Microsoft.AspNetCore.Identity;

namespace Ehrms.Authentication.API.Database.Models;

public class UserRole : IdentityUserRole<Guid>
{
    public virtual User? User { get; set; }
    public virtual Role? Role { get; set; }
}