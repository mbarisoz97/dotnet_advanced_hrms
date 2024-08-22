namespace Ehrms.Authentication.API.Validation.User.Commands;

public class ResetUserPasswordCommandValidator : AbstractValidator<UpdateUserPasswordCommand>
{
    public ResetUserPasswordCommandValidator()
    {
        RuleFor(x => x.Username).NotEmpty();
        
        RuleFor(x => x.CurrentPassword)
            .NotEmpty()
            .MinimumLength(Consts.MinPasswordLength)
            .MaximumLength(Consts.MaxPasswordLength);
        
        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .MinimumLength(Consts.MinPasswordLength)
            .MaximumLength(Consts.MaxPasswordLength);
    }
}