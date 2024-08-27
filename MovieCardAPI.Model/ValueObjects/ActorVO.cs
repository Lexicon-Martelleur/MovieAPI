namespace MovieCardAPI.Model.ValueObjects;

public record class ActorVO(
    string Name,
    long DateOfBirth
) : PersonVO(Name, DateOfBirth);
