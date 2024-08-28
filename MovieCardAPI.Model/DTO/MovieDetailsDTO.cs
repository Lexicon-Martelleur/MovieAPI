using MovieCardAPI.Model.ValueObjects;

namespace MovieCardAPI.Model.DTO;

public record class MovieDetailsDTO(
    int Id,
    string Title,
    int Rating,
    long TimeStamp,
    string Description,
    DirectorDTO Directory,
    ActorDTO[] Genres,
    GenreDTO[] Actors
) : BaseMovieVO(Title, Rating, TimeStamp, Description);
