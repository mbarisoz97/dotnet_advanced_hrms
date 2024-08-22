namespace Ehrms.Authentication.API.Validation.User.Commands;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Username)
            .Length(Consts.MinUserNameLength, Consts.MaxUserNameLength);

        RuleFor(x => x.Email)
            .EmailAddress();

        RuleFor(x=>x.Password)
            .Length(Consts.MinPasswordLength, Consts.MaxPasswordLength);

        RuleFor(x=>x.Roles)
            .NotEmpty();
    }
}