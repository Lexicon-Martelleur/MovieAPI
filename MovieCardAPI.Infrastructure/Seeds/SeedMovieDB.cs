using Bogus;
using Microsoft.EntityFrameworkCore;
using MovieCardAPI.Constants;
using MovieCardAPI.Infrastructure.Contexts;
using MovieCardAPI.Entities;
using Bogus.DataSets;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace MovieCardAPI.Infrastructure.Seeds;

public static class SeedMovieDB
{
    public static async Task RunAsync(
        MovieContext context,
        IConfiguration configuration,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        if (await IsExistingMovieData(context)) {
            return;
        }

        Console.WriteLine("Seeding Data...");

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

        await CreateUserRolesAsync(roleManager);

        await CreateUsersAsync(20, configuration, userManager);

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


    private static async Task CreateUserRolesAsync(
        RoleManager<IdentityRole> roleManager)
    {
        foreach (var roleName in UserRoles.ALL_ROLES)
        {
            if (await roleManager.RoleExistsAsync(roleName)) continue;
            var role = new IdentityRole { Name = roleName };
            var result = await roleManager.CreateAsync(role);

            if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
        }
    }

    private static async Task CreateUsersAsync(
        int nrOfUsers,
        IConfiguration configuration,
        UserManager<ApplicationUser> userManager)
    {
        string[] positions = UserRoles.ALL_ROLES.ToArray();

        var faker = new Faker<ApplicationUser>("en").Rules((f, user) =>
        {
            user.Name = f.Person.FullName;
            user.Position = positions[f.Random.Int(0, positions.Length - 1)];
            user.Email = f.Person.Email;
            user.UserName = f.Person.UserName;
        });

        var users = faker.Generate(nrOfUsers);

        var passWord = AppConfig.GetPassword(configuration);

        foreach (var user in users)
        {
            var result = await userManager.CreateAsync(user, passWord);
            if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));

            if (user.Position == UserRoles.ADMIN)
            {
                await userManager.AddToRoleAsync(user, UserRoles.ADMIN);
            }
            else
            {
                await userManager.AddToRoleAsync(user, UserRoles.USER);
            }
        }
    }
}
