namespace Ehrms.Authentication.API.Validation.User.Commands;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Roles)
            .NotEmpty();
    }
}