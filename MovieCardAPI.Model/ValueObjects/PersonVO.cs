using MovieCardAPI.Constants;
using System.ComponentModel.DataAnnotations;

namespace MovieCardAPI.Model.ValueObjects;

public record class PersonVO(
    [MaxLength(PersonConstants.MAX_NAME)]
    [MinLength(PersonConstants.MIN_NAME)] string Name,
    long DateOfBirth
);
