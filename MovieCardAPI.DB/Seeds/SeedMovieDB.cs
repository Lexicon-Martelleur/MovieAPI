using Bogus;
using Microsoft.EntityFrameworkCore;
using MovieCardAPI.Constants;
using MovieCardAPI.DB.Contexts;
using MovieCardAPI.Entities;
using System.IO;

namespace MovieCardAPI.DB.Seeds;

public static class SeedMovieDB
{
    public static async Task RunAsync(MovieContext context)
    {
        if (await IsExistingData(context)) { return; }

        var genres = CreateGenres(10);
        await context.AddRangeAsync(genres);

        var directors = CreateDirectors(10);
        await context.AddRangeAsync(directors);

        var actors = CreateActors(40);
        await context.AddRangeAsync(actors);

        var movies = CreateMovies(50, directors);
        await context.AddRangeAsync(movies);

        var movieRoles = CreateMovieRoles(actors, movies);
        await context.AddRangeAsync(movieRoles);

        var movieGenres = CreateMovieGenres(genres, movies);
        await context.AddRangeAsync(movieGenres);

        await context.SaveChangesAsync();
    }

    private static async Task<bool> IsExistingData(MovieContext context)
    {
        return await context.Movies.AnyAsync();
    }

    private static IEnumerable<Genre> CreateGenres(int nrOfGenres)
    {
        var genre = new List<Genre>();

        for (int i = 0; i < nrOfGenres; i++)
        {
            var fakeGenre = new Faker<Genre>("en").Rules((faker, genre) =>
            {
                Array values = Enum.GetValues(typeof(MovieGenreType));
                Random random = new();
                int randomIndex = random.Next(values.Length);
                
                var randomGenre = values.GetValue(randomIndex) is MovieGenreType genreType
                    ? genreType
                    : default;

                genre.Name = randomGenre;
            });
            genre.Add(fakeGenre);
        }

        return genre;
    }

    private static IEnumerable<Director> CreateDirectors(int nrOfDirectors)
    {
        var directors = new List<Director>();

        for (int i = 0; i < nrOfDirectors; i++)
        {
            var fakeDirecetor = new Faker<Director>("en").Rules((faker, director) =>
            {
                director.Name = $"{faker.Name.FirstName()} {faker.Name.LastName()}";
                director.DateOfBirth = ((DateTimeOffset)faker.Date.Recent()).ToUnixTimeSeconds();
                director.ContactInformation = new()
                {
                    Email = faker.Internet.Email(),
                    PhoneNumber = faker.Phone.PhoneNumber(),

                };
            });
            directors.Add(fakeDirecetor);
        }

        return directors;
    }

    private static IEnumerable<Actor> CreateActors(int nrOfDirectors)
    {
        var actors = new List<Actor>();

        for (int i = 0; i < nrOfDirectors; i++)
        {
            var fakeActor = new Faker<Actor>("en").Rules((faker, actor) =>
            {
                actor.Name = $"{faker.Name.FirstName()} {faker.Name.LastName()}";
                actor.DateOfBirth = ((DateTimeOffset)faker.Date.Recent()).ToUnixTimeSeconds();
            });
            actors.Add(fakeActor);
        }

        return actors;
    }

    private static IEnumerable<Movie> CreateMovies(
        int nrOfMovies,
        IEnumerable<Director> directors)
    {
        var movies = new List<Movie>();
        var random = new Random();

        for (int i = 0; i < nrOfMovies; i++)
        {
            int randomDirectorIndex = random.Next(0, directors.Count());

            var fakeMovie = new Faker<Movie>("en").Rules((faker, movie) =>
            {
                movie.Title = faker.Lorem.Word();
                movie.Rating = faker.Random.Int(1, 5);
                movie.TimeStamp = ((DateTimeOffset)faker.Date.Recent()).ToUnixTimeSeconds();
                movie.Description = faker.Lorem.Text();
                movie.Director = directors.ElementAt(randomDirectorIndex);
            });
            movies.Add(fakeMovie);
        }

        return movies;
    }

    /**
     * TODO Make Seeding of Actors random
     */
    private static IEnumerable<MovieRole> CreateMovieRoles(
        IEnumerable<Actor> actors,
        IEnumerable<Movie> movies)
    {
        var movieRoles = new List<MovieRole>();

        var random = new Random();

        foreach (var movie in movies)
        {
            int randomNrOfActors = random.Next(2, 10);

            for (int i = 0; i < randomNrOfActors; i++)
            {
                var movieRole = new MovieRole
                {
                    Movie = movie,
                    Actor = actors.ElementAt(i),
                };

                movieRoles.Add(movieRole);
            }
        }
        return movieRoles;
    }

    /**
     * TODO Make Seeding of Genres random
     */
    private static IEnumerable<MovieGenre> CreateMovieGenres(
        IEnumerable<Genre> genres,
        IEnumerable<Movie> movies)
    {
        var movieGenres = new List<MovieGenre>();

        var random = new Random();

        foreach (var movie in movies)
        {
            int randomNrOfGenres = random.Next(1, 4);

            for (int i = 0; i < randomNrOfGenres; i++)
            {
                var movieRole = new MovieGenre
                {
                    Movie = movie,
                    Genre = genres.ElementAt(i),
                };

                movieGenres.Add(movieRole);
            }
        }
        return movieGenres;
    }
}
