using Microsoft.AspNetCore.Identity;
using Ehrms.Authentication.API.Database.Models;

namespace Ehrms.Authentication.API.Adapter;

public interface IUserManagerAdapter
{
    IQueryable<User> Users { get; }

    Task<IdentityResult> AddUserToRole(User user, string role);
    Task<IdentityResult> AddUserToRoles(User user, IEnumerable<string> roles);
    Task<IdentityResult> CreateAsync(User user, string password);
    Task<IdentityResult> UpdateAsync(User user);
}

public class UserManagerAdapter : IUserManagerAdapter
{
    private readonly UserManager<User> _userManager;

    public UserManagerAdapter(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public IQueryable<User> Users => _userManager.Users;

    public async Task<IdentityResult> CreateAsync(User user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }

    public async Task<IdentityResult> UpdateAsync(User user)
    {
        return await _userManager.UpdateAsync(user);
    }

    public async Task<IdentityResult> AddUserToRole(User user, string role)
    {
        return await _userManager.AddToRoleAsync(user, role);
    }

    public async Task<IdentityResult> AddUserToRoles(User user, IEnumerable<string> roles)
    {
        return await _userManager.AddToRolesAsync(user, roles);
    }
}