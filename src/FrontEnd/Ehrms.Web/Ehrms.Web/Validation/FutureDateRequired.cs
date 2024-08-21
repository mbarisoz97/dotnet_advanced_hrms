using System.ComponentModel.DataAnnotations;

namespace Ehrms.Web.Validation;

public class FutureDateRequired : ValidationAttribute
{
    private readonly string _comparisonProperty;

    public FutureDateRequired(string comparisonProperty)
    {
        _comparisonProperty = comparisonProperty;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
        if (property == null)
        {
            return new ValidationResult($"Property {_comparisonProperty} not found.");
        }

        var startsAtValue = (DateTime?)property.GetValue(validationContext.ObjectInstance);
        var endsAtValue = (DateTime?)value;

        var validationResult = endsAtValue >= startsAtValue
            ? ValidationResult.Success
            : new ValidationResult("End date must be later than the start date.", [validationContext.DisplayName]);

        return validationResult;
    }
}