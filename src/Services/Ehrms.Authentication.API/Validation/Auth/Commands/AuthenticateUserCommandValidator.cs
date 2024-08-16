using Ehrms.Authentication.API.Handlers.Auth.Commands;

namespace Ehrms.Authentication.API.Validation.Auth.Commands;

public class AuthenticateUserCommandValidator : AbstractValidator<AuthenticateUserCommand>
{
    public AuthenticateUserCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}