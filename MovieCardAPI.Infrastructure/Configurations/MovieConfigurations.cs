using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieCardAPI.Entities;

namespace MovieCardAPI.Infrastructure.Configurations;

internal class MovieConfigurations : IEntityTypeConfiguration<Movie>
{
    internal const string UpdateProperty = "Updated";
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.Property<long>(UpdateProperty);

        builder.HasMany(e => e.Actors)
            .WithMany(e => e.Movies)
            .UsingEntity<MovieRole>();

        builder.HasMany(e => e.Genres)
            .WithMany(e => e.Movies)
            .UsingEntity<MovieGenre>();
    }
}
