using System.ComponentModel.DataAnnotations;

namespace MovieCardAPI.Model.DTO;

public record UserRegistrationDTO
{
    [Required(ErrorMessage = "UserName is required")]
    public string UserName { get; init; } = String.Empty;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; init; } = String.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    public string Email { get; init; } = String.Empty;

    [Required]
    public string Name { get; init; } = String.Empty;


    [Required]
    public string Position { get; init; } = String.Empty;

}
