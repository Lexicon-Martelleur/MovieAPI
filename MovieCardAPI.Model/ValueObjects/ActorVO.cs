namespace MovieCardAPI.Model.ValueObjects;

public record class ActorVO(
    int Id,
    string Name,
    long DateOfBirth
) : PersonVO(Id, Name, DateOfBirth);
