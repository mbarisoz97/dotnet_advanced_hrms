using Microsoft.AspNetCore.Identity;

namespace Ehrms.Authentication.API.Database.Models;

public sealed class User : IdentityUser<Guid>
{
    public bool IsActive { get; set; } = false;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpiry { get; set; }
    public ICollection<Role> Roles { get; set; } = [];
}