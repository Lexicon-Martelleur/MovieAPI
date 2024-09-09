using MovieCardAPI.Model.Validation;
using MovieCardAPI.Model.ValueObjects;

namespace MovieCardAPI.Model.DTO;

public class DirectorDTO : IPerson
{
    public required int Id { get; set; }

    [ValidateName]
    public required string Name { get; set; }

    [UNIXTimestampValidation]
    public required long DateOfBirth { get; set; }

    public ContactInformationDTO? ContactInformation { get; set; }
}
