using Ehrms.Authentication.API.Adapter;
using Ehrms.Authentication.API.Exceptions;

namespace Ehrms.Authentication.API.Handlers.User.Commands;

public sealed class UpdateUserPasswordCommand : IRequest<Result<Database.Models.User?>>
{
    public string Username { get; set; } = string.Empty;
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}

internal sealed class UpdateUserPasswordCommandHandler : IRequestHandler<UpdateUserPasswordCommand, Result<Database.Models.User?>>
{
    private readonly IUserManagerAdapter _userManagerAdapter;

    public UpdateUserPasswordCommandHandler(IUserManagerAdapter userManagerAdapter)
    {
        _userManagerAdapter = userManagerAdapter;
    }

    public async Task<Result<Database.Models.User?>> Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManagerAdapter.FindByNameAsync(request.Username);
        if (user == null)
        {
            return new Result<Database.Models.User?>(
                new UserNotFoundException($"Could not find user with username <{request.Username}>"));
        }

        if (!user.IsActive)
        {
            return new Result<Database.Models.User?>(
                new UserAccountInactiveException($"User <{request.Username}> is not active"));
        }
        
        var isPasswordCheckSucceeded = await _userManagerAdapter.CheckPasswordAsync(user, request.CurrentPassword);
        if (!isPasswordCheckSucceeded)
        {
            return new Result<Database.Models.User?>(
                new UserCredentialsInvalidException("Current password was not correct"));
        }

        var resetToken = await _userManagerAdapter.GeneratePasswordResetTokenAsync(user);
        var passwordResetResult = await _userManagerAdapter.ResetPasswordAsync(user, resetToken, request.NewPassword);
        if (!passwordResetResult.Succeeded)
        {
            return new Result<Database.Models.User?>(
                new UserPasswordResetFailedException());
        }

        user.MustChangePassword = false;
        await _userManagerAdapter.UpdateAsync(user);
        
        return new Result<Database.Models.User?>(user);
    }
}