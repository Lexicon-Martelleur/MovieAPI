namespace MovieCardAPI.Model.DTO;

public record DirectorDTO(
    int Id,
    string Name,
    long DateOfBirth,
    ContactInformationDTO ContactInformation
);
