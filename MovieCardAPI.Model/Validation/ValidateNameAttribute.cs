using MovieCardAPI.Constants;
using MovieCardAPI.Model.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace MovieCardAPI.Model.Validation;

public class ValidateNameAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(
        object? value,
        ValidationContext validationContext)
    {
        if (value is not string input ||
            validationContext.ObjectInstance is not IPerson person)
        {
            return new ValidationResult($"Validation {nameof(ValidateNameAttribute)} error");
        }

        var errorMessage = $"Name size must be in interval [{PersonConstants.MIN_NAME}, {PersonConstants.MAX_NAME}]";

        return (person.Name.Trim().Length < PersonConstants.MIN_NAME ||
            person.Name.Trim().Length > PersonConstants.MAX_NAME)
            ? new ValidationResult(errorMessage)
            : ValidationResult.Success;
    }
}
