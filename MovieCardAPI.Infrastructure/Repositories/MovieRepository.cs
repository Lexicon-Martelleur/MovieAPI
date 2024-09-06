using Microsoft.EntityFrameworkCore;
using MovieCardAPI.Infrastructure.Contexts;
using MovieCardAPI.Entities;
using MovieCardAPI.Model.Repository;

namespace MovieCardAPI.Infrastructure.Repositories;

public class MovieRepository : BaseRepository<Movie>, IMovieRepository
{
    public MovieRepository(MovieContext context) : base(context) {}

    public async Task<Movie?> GetMovie(int id)
    {
        return await Context.Movies
            .Where(item => item.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Movie>> GetMovies()
    {
        return await Context.Movies.ToListAsync();
    }

    public async Task<bool> IsExistingDirector(int id)
    {
        return await Context.Directors.FirstOrDefaultAsync(i => i.Id == id) != null;
    }

    public async Task<bool> IsExistingActors(IEnumerable<int> ids)
    {
        var matchingIds = await Context.Actors
            .Where(actor => ids.Contains(actor.Id))
            .Select(actor => actor.Id)
            .ToListAsync();

        return matchingIds.Count == ids.Distinct().ToList().Count;
    }

    public async Task<bool> IsExistingGenres(IEnumerable<int> ids)
    {
        var matchingIds = await Context.Genres
            .Where(genre => ids.Contains(genre.Id))
            .Select(genre => genre.Id)
            .ToListAsync();

        return matchingIds.Count == ids.Distinct().ToList().Count;
    }

    public void CreateMovie(
        Movie movie,
        IEnumerable<int> actorIds,
        IEnumerable<int> genreIds)
    {
        Context.Movies.Add(movie);
    }

    public void CreateMovieRoles(Movie movie, IEnumerable<int> actorIds)
    {
        foreach (var id in actorIds)
        {
            Context.MovieRoles.Add(new MovieRole()
            {
                MovieId = movie.Id,
                ActorId = id,
            });
        }
    }

    public void CreateMovieGenres(Movie movie, IEnumerable<int> genreIds)
    {
        foreach (var id in genreIds)
        {
            Context.MovieGenres.Add(new MovieGenre()
            {
                MovieId = movie.Id,
                GenreId = id
            });
        }
    }

    public async Task RemoveMovieRoles(int movieId)
    {
        Context.MovieRoles.RemoveRange(await Context.MovieRoles
            .Where(item => item.MovieId == movieId)
            .ToListAsync()
        );
    }

    public void UpdateMovieRoles(
        IEnumerable<int> newActorIds,
        int movieId)
    {
        foreach (var actorId in newActorIds)
        {
            Context.MovieRoles.Add(new()
            {
                MovieId = movieId,
                ActorId = actorId
            });
        }
    }

    public async Task RemoveMovieGenres(int movieId)
    {
        Context.MovieGenres.RemoveRange(await Context.MovieGenres
            .Where(item => item.MovieId == movieId)
            .ToListAsync()
        );
    }

    public void UpdateMovieGenres(
        IEnumerable<int> newGenreIds,
        int movieId)
    {
        foreach (var genreId in newGenreIds)
        {
            Context.MovieGenres.Add(new()
            {
                MovieId = movieId,
                GenreId = genreId
            });
        }
    }

    public async Task DeleteMovie(int id)
    {
        var movieEntity = await Context.Movies
            .Where(item => item.Id == id)
            .FirstOrDefaultAsync();

        if (movieEntity == default)
        {
            return;
        }
        Context.Movies.Remove(movieEntity);
    }

    public async Task<IEnumerable<Genre>> GetMovieGenres(int movieId)
    {
        return await Context.MovieGenres
            .Where(moviGenre => moviGenre.MovieId == movieId)
            .Join(
                Context.Genres,
                moviGenre => moviGenre.GenreId,
                genre => genre.Id,
                (moviGenre, genre) => genre
            )
            .ToListAsync();
    }

    public async Task<Director?> GetDirector(int movieId)
    {
        return await Context.Movies
            .Where(movie => movie.Id == movieId)
            .Select(movie => movie.Director)
            .FirstOrDefaultAsync();
    }

    public async Task<ContactInformation?> GetContactInformation(int directorId)
    {
        return await Context.Directors
            .Where(director => director.Id == directorId)
            .Select(director => director.ContactInformation)
            .FirstOrDefaultAsync();
    }
}
