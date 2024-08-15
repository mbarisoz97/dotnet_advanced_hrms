namespace Ehrms.Authentication.API.Validation.User.Commands;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x=>x.Username)
            .MinimumLength(3)
            .MaximumLength(50);

        RuleFor(x => x.Email)
            .EmailAddress();

        RuleFor(x=>x.Password)
            .MinimumLength(8)
            .MaximumLength(50);

        RuleFor(x=>x.Roles)
            .NotEmpty();
    }
}