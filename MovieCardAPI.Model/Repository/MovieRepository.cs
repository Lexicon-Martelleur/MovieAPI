using Microsoft.EntityFrameworkCore;
using MovieCardAPI.Entities;
using MovieCardAPI.DB.Contexts;
using MovieCardAPI.Constants;

namespace MovieCardAPI.Model.Repository;

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

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() >= 0;
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

    public async Task CreateMovie(
        Movie movie,
        IEnumerable<int> actorIds,
        IEnumerable<int> genreIds)
    {
        _context.Movies.Add(movie);
        await SaveChangesAsync();
        
        foreach (var id in actorIds)
        {
            _context.MovieRoles.Add(new MovieRole()
            {
                MovieId = movie.Id,
                ActorId = id,
            });
        }

        foreach (var id in genreIds)
        {
            _context.MovieGenres.Add(new MovieGenre()
            {
                MovieId = movie.Id,
                GenreId = id
            });
        }
    }

    public async Task UpdateMovieRoles(
        IEnumerable<int> newActorIds,
        int movieId)
    {
        var oldActorIds = await _context.MovieRoles
            .Where(item => item.MovieId == movieId)
            .Select(item => item.MovieId)
            .ToListAsync();

        foreach (var actorId in oldActorIds) {
            var existingMovieRole = await _context.MovieRoles
                .FirstOrDefaultAsync(role => 
                    role.MovieId == movieId);
            if (existingMovieRole != null) { 
                _context.MovieRoles.Remove(existingMovieRole);
            }
            await SaveChangesAsync();
        }

        foreach (var actorId in newActorIds)
        {
            var movieRoleEntry = new MovieRole
            {
                MovieId = movieId,
                ActorId = actorId
            };

            _context.MovieRoles.Add(movieRoleEntry);
        }
        await SaveChangesAsync();
    }

    public async Task UpdateMovieGenres(
        IEnumerable<int> newGenreIds,
        int movieId)
    {
        var oldGenresIds = await _context.MovieGenres
            .Where(item => item.MovieId == movieId)
            .Select(item => item.MovieId)
            .ToListAsync();

        foreach (var genreId in oldGenresIds)
        {
            var existingMovieGenre = await _context.MovieGenres
                .FirstOrDefaultAsync(genre => 
                    genre.MovieId == movieId);
            if (existingMovieGenre != null)
            {
                _context.MovieGenres.Remove(existingMovieGenre);
            }
            await SaveChangesAsync();
        }

        foreach (var genreId in newGenreIds)
        {
            var movieGenreEntry = new MovieGenre
            {
                MovieId = movieId,
                GenreId = genreId
            };

            _context.MovieGenres.Add(movieGenreEntry);
        }
        await SaveChangesAsync();
    }
}
