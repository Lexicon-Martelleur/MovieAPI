using MovieCardAPI.Model.Validation;
using MovieCardAPI.Model.ValueObjects;

namespace MovieCardAPI.Model.DTO;

public class ActorDTO : IPerson
{
    public required int Id { get; set; }

    [ValidateName]
    public required string Name { get; set; } = string.Empty;
    
    public required long DateOfBirth { get; set; }
}
