using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MovieCardAPI.Entities;
public class ApplicationUser : IdentityUser
{
    [Required(ErrorMessage = "Name is a required field.")]
    [MaxLength(30, ErrorMessage = "Max length for the Name is 30 characters.")]
    public string Name { get; set; } = String.Empty;

    [Required(ErrorMessage = "Position is a required field.")]
    [MaxLength(20, ErrorMessage = "Maximum length for the Position is 20 characters.")]
    public string Position { get; set; } = String.Empty;

    public string? RefreshToken { get; set; }
        
    public DateTime RefreshTokenExpireTime { get; set; }

}
