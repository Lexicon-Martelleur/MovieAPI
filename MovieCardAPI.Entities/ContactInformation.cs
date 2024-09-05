using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MovieCardAPI.Entities;

public class ContactInformation
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    public string PhoneNumber { get; set; } = string.Empty;

    [ForeignKey(nameof(Director))]
    public int DirectorId { get; set; }

    // Navigation Props
    public Director Director { get; set; } = null!;
}
