using System.ComponentModel.DataAnnotations;

namespace MovieCardAPI.Entities;
using System.ComponentModel.DataAnnotations.Schema;

public class Movie
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Title { get; set; }

    [Required]
    [Range(1, 5)]
    public int Rating { get; set; }

    [Required]
    public long TimeStamp { get; set; }

    [Required]
    [MaxLength(10000)]
    public string Description { get; set; }

    [ForeignKey(nameof(Director))]
    public int DirectorId { get; set; }

    //Navigation props
    public Director Director { get; set; }

    public ICollection<Actor> Actors { get; set; }

    public ICollection<MovieRole> MovieRoles { get; set; }

    public ICollection<Genre> Genres { get; set; }

    public ICollection<MovieGenre> MovieGenre{ get; set; }
}
