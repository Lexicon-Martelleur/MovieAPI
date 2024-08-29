using Microsoft.EntityFrameworkCore;
using MovieCardAPI.DB.Configurations;
using MovieCardAPI.Entities;

namespace MovieCardAPI.DB.Contexts;

public class MovieContext : DbContext
{
    public DbSet<Movie> Movies { get; set; }

    public DbSet<Actor> Actors { get; set; }

    public DbSet<Director> Directors { get; set; }

    public DbSet<Genre> Genres { get; set; }

    public DbSet<ContactInformation> ContactInformations { get; set; }

    public DbSet<MovieGenre> MovieGenres { get; set; }

    public DbSet<MovieRole> MovieRoles { get; set; }

    public MovieContext(DbContextOptions<MovieContext> options)
        : base(options)
    { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        if (environment == "Development")
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new MovieConfigurations());
        builder.ApplyConfiguration(new MovieRoleConfigurations());
        builder.ApplyConfiguration(new MovieGenreConfigurations());     
    }
}
