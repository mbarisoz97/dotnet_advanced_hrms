using Ehrms.Authentication.API.Handlers.Auth.Commands;

namespace Ehrms.Authentication.API.Validation.Auth.Commands;

public class RefreshUserAuthenticationCommandValidator : AbstractValidator<RefreshAuthenticationCommand>
{
    public RefreshUserAuthenticationCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty();

        RuleFor(x=>x.AccessToken)
            .NotEmpty();
    }
}