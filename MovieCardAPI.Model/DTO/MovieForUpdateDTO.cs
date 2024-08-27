namespace MovieCardAPI.Model.DTO;

public record class MovieForUpdateDTO(
    string Title,
    int Rating,
    long TimeStamp,
    string Description,
    int DirectorId,
    int[] ActorIds,
    int[] GenreIds
) : BaseMovieDTO(Title, Rating, TimeStamp, Description);
