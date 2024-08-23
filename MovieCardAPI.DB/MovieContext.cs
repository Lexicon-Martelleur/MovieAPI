using Microsoft.EntityFrameworkCore;
using MovieCardAPI.Entities;

namespace MovieCardAPI.DB;

public class MovieContext: DbContext
{
    public DbSet<Movie> Movies { get; set; }

    public MovieContext(DbContextOptions<MovieContext> options)
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Movie>().HasData(
            new Movie()
            {
                Id = 1,
                Title = "T1",
                Rating = 1,
                TimeStamp = 1724414949,
                Description = "d1"
            },
            new Movie()
            {
                Id = 2,
                Title = "T2",
                Rating = 2,
                TimeStamp = 1724414949,
                Description = "d2"
            },
            new Movie()
            {
                Id = 3,
                Title = "T3",
                Rating = 3,
                TimeStamp = 1724414949,
                Description = "d3"
            }
        );
    }
}
