using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MovieCardAPI.Entities;

namespace MovieCardAPI.DB.Configurations;

internal class MovieRoleConfigurations : IEntityTypeConfiguration<MovieRole>
{
    public void Configure(EntityTypeBuilder<MovieRole> builder)
    {
        builder.HasKey(item => new { item.MovieId, item.ActorId });
    }
}
