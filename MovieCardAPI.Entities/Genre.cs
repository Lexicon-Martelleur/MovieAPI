using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MovieCardAPI.Entities;

public class Genre
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    // Navigation Prop
    public ICollection<Movie> Movies { get; set; }

    public ICollection<MovieGenre> MovieGenre { get; set; }
}
