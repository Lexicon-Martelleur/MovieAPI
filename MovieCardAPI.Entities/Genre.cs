using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

using MovieCardAPI.Constants;

namespace MovieCardAPI.Entities;

public class Genre
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public MovieGenreType Name { get; set; }

    // Navigation Prop
    public ICollection<Movie> Movies { get; set; } = [];

    public ICollection<MovieGenre> MovieGenre { get; set; } = [];
}
