using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieCardAPI.Entities;

namespace MovieCardAPI.DB.Configurations;

internal class MovieConfigurations : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        // builder.Property<long>("Updated");

        builder.HasMany(e => e.Actors)
            .WithMany(e => e.Movies)
            .UsingEntity<MovieRole>();

        builder.HasMany(e => e.Genres)
            .WithMany(e => e.Movies)
            .UsingEntity<MovieGenre>();
    }
}
