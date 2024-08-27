namespace MovieCardAPI.Model.DTO;

public record class MovieForUpdateDTO(
    string Title,
    int Rating,
    long TimeStamp,
    string Description,
    int[] ActorIds,
    int[] Genres
) : BaseMovieDTO(Title, Rating, TimeStamp, Description);
