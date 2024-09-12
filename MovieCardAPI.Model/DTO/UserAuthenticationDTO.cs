using System.ComponentModel.DataAnnotations;

namespace MovieCardAPI.Model.DTO;
public record UserAuthenticationDTO
{
    [Required]
    public string? UserName { get; init; }

    [Required]
    public string? Password { get; init; }
}
