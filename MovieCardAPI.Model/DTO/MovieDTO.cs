using MovieCardAPI.Model.Validation;
using MovieCardAPI.Model.ValueObjects;

namespace MovieCardAPI.Model.DTO;

public record class MovieDTO : IMovie {
    public required int Id { get; set; }

    [ValidateMovieTitle]
    public required string Title { get; set; }

    [ValidateMovieRating]
    public required int Rating { get; set; }

    public required long TimeStamp { get; set; }

    [ValidateMovieDescription]
    public required string Description { get; set; }
}
