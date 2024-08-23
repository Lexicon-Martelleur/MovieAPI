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
    [MaxLength(1000)]
    public long TimeStamp { get; set; }

    [Required]
    [MaxLength(10000)]
    public string Description { get; set; }
}
