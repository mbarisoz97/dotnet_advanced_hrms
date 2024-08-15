using Microsoft.AspNetCore.Identity;

namespace Ehrms.Authentication.API.Adapter;

public interface IUserManagerAdapter
{
    IQueryable<User> Users { get; }

    Task<IdentityResult> AddUserToRole(User user, string role);
    Task<IdentityResult> AddToRolesAsync(User user, IEnumerable<string> roles);
    Task<IdentityResult> CreateAsync(User user, string password);
    Task<IdentityResult> UpdateAsync(User user);
    Task<IdentityResult> RemoveFromRolesAsync(User user, IEnumerable<string> roles);
    Task<User?> FindByNameAsync(string username);
    Task<bool> CheckPasswordAsync(User user, string password);
}
