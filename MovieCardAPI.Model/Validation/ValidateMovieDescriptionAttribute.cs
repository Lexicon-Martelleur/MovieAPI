using MovieCardAPI.Constants;
using MovieCardAPI.Model.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace MovieCardAPI.Model.Validation;

public class ValidateMovieDescriptionAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(
        object? value,
        ValidationContext validationContext)
    {
        if (value is not string input ||
            validationContext.ObjectInstance is not IMovie movie)
        {
            return new ValidationResult($"Validation {nameof(ValidateNameAttribute)} error");
        }

        var errorMessage = $"Movie description size must be in interval [{MovieConstants.MIN_TEXT}, {MovieConstants.MAX_DESCRIPTION}]";

        return (movie.Description.Trim().Length < MovieConstants.MIN_TEXT ||
            movie.Description.Trim().Length > MovieConstants.MAX_DESCRIPTION)
            ? new ValidationResult(errorMessage)
            : ValidationResult.Success;
    }
}
