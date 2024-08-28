using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MovieCardAPI.Constants;

namespace MovieCardAPI.Entities;

public class Director
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(PersonConstants.MAX_NAME)]
    [MinLength(PersonConstants.MIN_NAME)]
    public string Name { get; set; }

    [Required]
    public long DateOfBirth { get; set; }

    // Navigation Props
    public ContactInformation ContactInformation { get; set; }

    public ICollection<Movie> Movies { get; set; }
}
