namespace MovieCardAPI.Model.ValueObjects;

public interface IPerson {
    string Name { get; set; }

    long DateOfBirth { get; set; }
};
