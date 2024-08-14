using Microsoft.AspNetCore.Identity;

namespace Ehrms.Authentication.API.Adapter;

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

    public async Task<IdentityResult> AddToRolesAsync(User user, IEnumerable<string> roles)
    {
        return await _userManager.AddToRolesAsync(user, roles);
    }

    public async Task<IdentityResult> RemoveFromRolesAsync(User user, IEnumerable<string> roles)
    {
        return await _userManager.RemoveFromRolesAsync(user, roles);
    }

    public async Task<User?> FindByNameAsync(string username)
    {
       return await _userManager.FindByNameAsync(username);
    }

    public async Task<bool> CheckPasswordAsync(User user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }
}