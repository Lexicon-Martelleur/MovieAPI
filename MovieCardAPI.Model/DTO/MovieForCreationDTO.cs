namespace MovieCardAPI.Model.DTO;

public record class MovieForCreationDTO(
    string Title,
    int Rating,
    long TimeStamp,
    string Description,
    int DirectorId,
    int[] ActorIds,
    int[] Genres
) : BaseMovieDTO(Title, Rating, TimeStamp, Description);
