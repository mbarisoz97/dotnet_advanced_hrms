using FluentValidation;
using Ehrms.EmployeeInfo.API.Handlers.Title.Command;

namespace Ehrms.EmployeeInfo.API.Validation.Title;

public class UpdateTitleCommandValidator : AbstractValidator<UpdateTitleCommand>
{
    public UpdateTitleCommandValidator()
    {
        RuleFor(x => x.TitleName)
            .NotEmpty()
            .MinimumLength(Consts.MinTitleNameLength)
            .MaximumLength(Consts.MaxTitleNameLength);
    }
}