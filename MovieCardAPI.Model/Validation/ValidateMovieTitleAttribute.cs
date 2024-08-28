using MovieCardAPI.Constants;
using MovieCardAPI.Model.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace MovieCardAPI.Model.Validation;

public class ValidateMovieTitleAttribute : ValidationAttribute
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

        var errorMessage = $"Movie title size must be in interval [{MovieConstants.MIN_TEXT}, {MovieConstants.MAX_TITLE}]";

        return (movie.Title.Trim().Length < MovieConstants.MIN_TEXT ||
            movie.Title.Trim().Length > MovieConstants.MAX_TITLE)
            ? new ValidationResult(errorMessage)
            : ValidationResult.Success;
    }
}
