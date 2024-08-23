namespace MovieCardAPI.Model.DTO;

public record class MovieCardDTO(
    int Id,
    string Title,
    int Rating,
    long TimeStamp,
    string Description
);
