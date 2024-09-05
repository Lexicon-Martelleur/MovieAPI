using Microsoft.EntityFrameworkCore;
using MovieCardAPI.Infrastructure.Contexts;
using MovieCardAPI.Entities;
using MovieCardAPI.Model.Repository;

namespace MovieCardAPI.Infrastructure.Repositories;

public class MovieRepository : IMovieRepository
{

    private readonly MovieContext _context;
    public MovieRepository(MovieContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Movie?> GetMovie(int id)
    {
        return await _context.Movies
            .Where(item => item.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Movie>> GetMovies()
    {
        return await _context.Movies.ToListAsync();
    }

    public async Task<bool> IsExistingDirector(int id)
    {
        return await _context.Directors.FirstOrDefaultAsync(i => i.Id == id) != null;
    }

    public async Task<bool> IsExistingActors(IEnumerable<int> ids)
    {
        var matchingIds = await _context.Actors
            .Where(actor => ids.Contains(actor.Id))
            .Select(actor => actor.Id)
            .ToListAsync();

        return matchingIds.Count == ids.Distinct().ToList().Count;
    }

    public async Task<bool> IsExistingGenres(IEnumerable<int> ids)
    {
        var matchingIds = await _context.Genres
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
        _context.Movies.Add(movie);
    }

    public void CreateMovieRoles(Movie movie, IEnumerable<int> actorIds)
    {
        foreach (var id in actorIds)
        {
            _context.MovieRoles.Add(new MovieRole()
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
            _context.MovieGenres.Add(new MovieGenre()
            {
                MovieId = movie.Id,
                GenreId = id
            });
        }
    }

    public async Task RemoveMovieRoles(
        IEnumerable<int> newActorIds,
        int movieId)
    {
        var removableRoles = await _context.MovieRoles
            .Where(item => item.MovieId == movieId)
            .ToListAsync();

        _context.MovieRoles.RemoveRange(removableRoles);
    }

    public void UpdateMovieRoles(
        IEnumerable<int> newActorIds,
        int movieId)
    {
        foreach (var actorId in newActorIds)
        {
            var movieRoleEntry = new MovieRole
            {
                MovieId = movieId,
                ActorId = actorId
            };

            _context.MovieRoles.Add(movieRoleEntry);
        }
    }

    public async Task RemoveMovieGenres(
        IEnumerable<int> newGenreIds,
        int movieId)
    {
        var removableGenres = await _context.MovieGenres
            .Where(item => item.MovieId == movieId)
            .ToListAsync();

        _context.MovieGenres.RemoveRange(removableGenres);
    }

    public void UpdateMovieGenres(
        IEnumerable<int> newGenreIds,
        int movieId)
    {
        foreach (var genreId in newGenreIds)
        {
            var movieGenreEntry = new MovieGenre
            {
                MovieId = movieId,
                GenreId = genreId
            };

            _context.MovieGenres.Add(movieGenreEntry);
        }
    }

    public async Task DeleteMovie(int id)
    {
        var movieEntity = await _context.Movies
            .Where(item => item.Id == id)
            .FirstOrDefaultAsync();

        if (movieEntity == default)
        {
            return;
        }
        _context.Movies.Remove(movieEntity);
    }

    public async Task<IEnumerable<Actor>> GetMovieRoles(int movieId)
    {
        return await _context.MovieRoles
            .Where(movieRole => movieRole.MovieId == movieId)
            .Join(
                _context.Actors,
                movieRole => movieRole.ActorId,
                actor => actor.Id,
                (moviRole, actor) => actor
            )
            .ToListAsync();
    }

    public async Task<IEnumerable<Genre>> GetMovieGenres(int movieId)
    {
        return await _context.MovieGenres
            .Where(moviGenre => moviGenre.MovieId == movieId)
            .Join(
                _context.Genres,
                moviGenre => moviGenre.GenreId,
                genre => genre.Id,
                (moviGenre, genre) => genre
            )
            .ToListAsync();
    }

    public async Task<Director?> GetDirector(int movieId)
    {
        return await _context.Movies
            .Where(movie => movie.Id == movieId)
            .Select(movie => movie.Director)
            .FirstOrDefaultAsync();
    }

    public async Task<ContactInformation?> GetContactInformation(int directorId)
    {
        return await _context.Directors
            .Where(director => director.Id == directorId)
            .Select(director => director.ContactInformation)
            .FirstOrDefaultAsync();
    }
}
