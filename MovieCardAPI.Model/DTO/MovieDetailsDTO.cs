using MovieCardAPI.Model.Validation;
using MovieCardAPI.Model.ValueObjects;

namespace MovieCardAPI.Model.DTO;

public record class MovieDetailsDTO {
    public required int Id { get; set; }
    
    [ValidateMovieTitle]
    public required string Title { get; set; }
    
    [ValidateMovieRating]
    public required int Rating { get; set; }
    
    public required long TimeStamp { get; set; }
    
    [ValidateMovieDescription]
    public required string Description { get; set; }

    public required DirectorDTO Director { get; set; }

    public required ActorDTO[] Actors { get; set; }

    public required GenreDTO[] Genres { get; set; }
}
