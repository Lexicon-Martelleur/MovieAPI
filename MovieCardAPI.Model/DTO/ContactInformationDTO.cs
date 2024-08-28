using System.ComponentModel.DataAnnotations;

namespace MovieCardAPI.Model.DTO;

public record ContactInformationDTO {

    [Length(1, 1000)]
    public required string Email { get; set; }

    [Length(1, 1000)]
    public required string PhoneNumber { get; set; }
}
