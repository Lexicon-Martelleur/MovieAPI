using Bogus;
using Microsoft.EntityFrameworkCore;
using MovieCardAPI.DB.Contexts;
using MovieCardAPI.Entities;

namespace MovieCardAPI.DB.Seeds;

public static class SeedMovieDB
{
    public static async Task RunAsync(MovieContext context)
    {
        if (await IsExistingData(context)) { return; }

        var movies = CreateMovies(20);
        await context.AddRangeAsync(movies);

        await context.SaveChangesAsync();
    }

    private static async Task<bool> IsExistingData(MovieContext context)
    {
        return await context.Movies.AnyAsync();
    }

    private static IEnumerable<Movie> CreateMovies(int nr)
    {
        var faker = new Faker<Movie>("en").Rules((faker, book) =>
        {
            book.Title = faker.Company.CatchPhrase();
            book.Rating = faker.Random.Int(1, 5);
            book.TimeStamp = ((DateTimeOffset)faker.Date.Recent()).ToUnixTimeSeconds();
            book.Description = faker.Lorem.Text();
        });

        return faker.Generate(nr);
    }
}
