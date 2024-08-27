namespace MovieCardAPI.Model.DTO;

public record class MovieDTO(
    int Id,
    string Title,
    int Rating,
    long TimeStamp,
    string Description
);
