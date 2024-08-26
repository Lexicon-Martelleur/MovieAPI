namespace MovieCardAPI.Entities;

public class MovieGenre
{
    public int MovieId { get; set; }
    public int GenreId { get; set; }

    // Navigation Props
    public Movie Movie { get; set; }
    public Genre Genre { get; set; }
}
