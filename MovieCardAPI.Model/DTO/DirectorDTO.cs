using MovieCardAPI.Model.Validation;
using MovieCardAPI.Model.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace MovieCardAPI.Model.DTO;

public class DirectorDTO : IPerson
{
    public required int Id { get; set; }

    [ValidateName]
    public required string Name { get; set; }

    [UNIXTimestampValidation]
    public required long DateOfBirth { get; set; }

    public required ContactInformationDTO ContactInformation { get; set; }
}
