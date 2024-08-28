using MovieCardAPI.Model.ValueObjects;

namespace MovieCardAPI.Model.DTO;

public record class ActorDTO(
    int Id,
    string Name,
    long DateOfBirth
) : PersonVO(Name, DateOfBirth);
