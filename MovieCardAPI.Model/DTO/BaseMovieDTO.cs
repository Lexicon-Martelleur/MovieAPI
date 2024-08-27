using MovieCardAPI.Constants;
using System.ComponentModel.DataAnnotations;

namespace MovieCardAPI.Model.DTO;

public abstract record class BaseMovieDTO(
    [MaxLength(MovieConstants.MAX_TITLE)] string Title,
    [Range(MovieConstants.MIN_RATING, MovieConstants.MAX_RATING)] int Rating,
    long TimeStamp,
    [MaxLength(MovieConstants.MAX_TITLE)]  string Description
);
