namespace MovieCardAPI.Model.ValueObjects;

public record class PersonVO(
    int Id,
    string Name,
    long DateOfBirth
);
