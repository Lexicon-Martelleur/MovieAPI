using MovieCardAPI.Model.ValueObjects;

namespace MovieCardAPI.Model.DTO;

public record class MovieForCreationDTO(
    string Title,
    int Rating,
    long TimeStamp,
    string Description,
    int DirectorId,
    int[] ActorIds,
    int[] GenreIds
) : BaseMovieVO(Title, Rating, TimeStamp, Description);
