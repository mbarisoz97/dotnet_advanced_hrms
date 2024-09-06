using FluentValidation;
using Ehrms.EmployeeInfo.API.Handlers.Title.Command;

namespace Ehrms.EmployeeInfo.API.Validation.Title;

public class CreateTitleCommandValidator : AbstractValidator<CreateTitleCommand>
{
    public CreateTitleCommandValidator()
    {
        RuleFor(x => x.TitleName)
            .NotEmpty()
            .MinimumLength(Consts.MinTitleNameLength)
            .MaximumLength(Consts.MaxTitleNameLength);
    }
}