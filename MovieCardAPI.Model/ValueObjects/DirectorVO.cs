namespace MovieCardAPI.Model.ValueObjects;

public record class DirectorVO(
    int Id,
    string Name,
    long DateOfBirth
);
