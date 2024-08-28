using MovieCardAPI.Constants;
using MovieCardAPI.Model.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace MovieCardAPI.Model.Validation;

public class ValidateMovieRatingAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(
        object? value,
        ValidationContext validationContext)
    {
        if (value is not int input ||
            validationContext.ObjectInstance is not IMovie movie)
        {
            return new ValidationResult($"Validation {nameof(ValidateNameAttribute)} error");
        }

        var errorMessage = $"Rating must be in interval [{MovieConstants.MIN_RATING}, {MovieConstants.MAX_RATING}]";

        return (movie.Rating < MovieConstants.MIN_RATING ||
            movie.Rating > MovieConstants.MAX_RATING)
            ? new ValidationResult(errorMessage)
            : ValidationResult.Success;
    }
}
