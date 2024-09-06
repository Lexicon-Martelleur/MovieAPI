using Microsoft.EntityFrameworkCore;
using MovieCardAPI.Infrastructure.Configurations;
using MovieCardAPI.Entities;

namespace MovieCardAPI.Infrastructure.Contexts;

public class MovieContext : DbContext
{
    public DbSet<Actor> Actors { get; set; }

    public DbSet<ContactInformation> ContactInformations { get; set; }

    public DbSet<Director> Directors { get; set; }

    public DbSet<Genre> Genres { get; set; }

    public DbSet<Movie> Movies { get; set; }

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

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ChangeTracker.DetectChanges();
        var modifiedMovies = ChangeTracker.Entries<Movie>().Where(movie => 
            movie.State == EntityState.Modified);

        foreach (var movie in modifiedMovies)
        {
            var unixTime = DateTimeOffset.Now.ToUnixTimeSeconds();
            movie.Property(MovieConfigurations.UpdateProperty)
                .CurrentValue = unixTime;
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
