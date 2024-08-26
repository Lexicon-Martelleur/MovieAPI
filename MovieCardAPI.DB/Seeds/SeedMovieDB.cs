using Bogus;
using Microsoft.EntityFrameworkCore;

using MovieCardAPI.Constants;
using MovieCardAPI.DB.Contexts;
using MovieCardAPI.Entities;

namespace MovieCardAPI.DB.Seeds;

public static class SeedMovieDB
{
    public static async Task RunAsync(MovieContext context)
    {
        if (await IsExistingMovieData(context)) {
            return;
        }

        Console.WriteLine("Seeding Data");

        var genres = CreateGenres();
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

    private static async Task<bool> IsExistingMovieData(MovieContext context)
    {
        return await context.Movies.AnyAsync();
    }

    private static IEnumerable<Genre> CreateGenres()
    {
        var genres = new List<Genre>();

        foreach (MovieGenreType genre in Enum.GetValues(typeof(MovieGenreType)))
        {
            genres.Add(new Genre() { Name = genre });
        }

        return genres;
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

    private static IEnumerable<MovieRole> CreateMovieRoles(
        IEnumerable<Actor> actors,
        IEnumerable<Movie> movies)
    {
        var movieRoles = new List<MovieRole>();

        var random = new Random();

        foreach (var movie in movies)
        {
            var availableActors = actors.ToList();
            var selectedActors = new List<Actor>();
            int randomNrOfActors = random.Next(1, Math.Min(4, availableActors.Count));

            for (int i = 0; i < randomNrOfActors; i++)
            {
                int randomIndex = random.Next(availableActors.Count);
                var selectedActor = availableActors[randomIndex];
                selectedActors.Add(selectedActor);
                availableActors.RemoveAt(randomIndex);
            }

            movieRoles.AddRange(selectedActors.Select(actor => new MovieRole
            {
                Movie = movie,
                Actor = actor,
            }));
        }

        return movieRoles;
    }

    private static IEnumerable<MovieGenre> CreateMovieGenres(
        IEnumerable<Genre> genres,
        IEnumerable<Movie> movies)
    {
        var movieGenres = new List<MovieGenre>();
        var random = new Random();

        foreach (var movie in movies)
        {
            var availableGenres = genres.ToList();
            var selectedGenres = new List<Genre>();
            int randomNrOfGenres = random.Next(1, Math.Min(4, availableGenres.Count));

            for (int i = 0; i < randomNrOfGenres; i++)
            {
                int randomIndex = random.Next(availableGenres.Count);
                var selectedGenre = availableGenres[randomIndex];
                selectedGenres.Add(selectedGenre);
                availableGenres.RemoveAt(randomIndex);
            }

            movieGenres.AddRange(selectedGenres.Select(genre => new MovieGenre
            {
                Movie = movie,
                Genre = genre,
            }));
        }

        return movieGenres;
    }
}
