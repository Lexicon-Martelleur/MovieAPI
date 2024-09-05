namespace MovieCardAPI.Entities;

public class MovieRole
{
    public int MovieId { get; set; }
    public int ActorId { get; set; }

    // Navigation Props
    public Movie Movie { get; set; } = null!;
    public Actor Actor { get; set; } = null!;
}
