using MovieCardAPI.Constants;

namespace MovieCardAPI.Model.DTO;

public record class GenreDTO(
    int Id,
    MovieGenreType NameAsNumber,
    string? NameAsString
);
