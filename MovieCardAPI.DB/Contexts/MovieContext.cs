﻿using Microsoft.EntityFrameworkCore;
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
        DefineMovieRoleKey(builder);
        DefineMovieGenreKey(builder);
        DescribeMovieRolesTable(builder);
        DescribeMovieGenreTable(builder);        
    }

    private static void DefineMovieRoleKey(ModelBuilder builder)
    {
        builder.Entity<MovieRole>()
            .HasKey(item => new { item.MovieId, item.ActorId });
    }

    private static void DefineMovieGenreKey(ModelBuilder builder)
    {
        builder.Entity<MovieGenre>()
            .HasKey(item => new { item.MovieId, item.GenreId });
    }

    private static void DescribeMovieRolesTable(ModelBuilder builder)
    {
        builder.Entity<Movie>()
            .HasMany(e => e.Actors)
            .WithMany(e => e.Movies)
            .UsingEntity<MovieRole>();
    }

    private static void DescribeMovieGenreTable(ModelBuilder builder)
    {
        builder.Entity<Movie>()
            .HasMany(e => e.Genres)
            .WithMany(e => e.Movies)
            .UsingEntity<MovieGenre>();
    }
}
