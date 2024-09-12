using System.ComponentModel.DataAnnotations;

namespace MovieCardAPI.Model.DTO;
public class UserAuthenticationDTO
{
    [Required]
    public string? UserName { get; init; }

    [Required]
    public string? Password { get; init; }
}
