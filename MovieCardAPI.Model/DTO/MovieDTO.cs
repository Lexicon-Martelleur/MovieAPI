using MovieCardAPI.Model.ValueObjects;

namespace MovieCardAPI.Model.DTO;

public record class MovieDTO(
    int Id,
    string Title,
    int Rating,
    long TimeStamp,
    string Description
) : BaseMovieDTO(Title, Rating, TimeStamp, Description);
