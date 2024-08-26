using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MovieCardAPI.Entities;

public class Actor
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Name { get; set; }

    [Required]
    public long DateOfBirth { get; set; }

    // Navigation Prop
    public ICollection<Movie> Movies { get; set; }

    public ICollection<MovieRole> MovieRoles { get; set; }
}
