using MovieCardAPI.Entities;

namespace MovieCardAPI.Model.Repository;

public interface IMovieRepository
{
    Task<IEnumerable<Movie>> GetMovies();

    Task<Movie?> GetMovie(int id);

    Task<bool> IsExistingDirector(int id);

    Task<bool> IsExistingActors(IEnumerable<int> ids);

    Task<bool> IsExistingGenres(IEnumerable<int> ids);

    Task CreateMovie(
        Movie movie,
        IEnumerable<int> actorIds,
        IEnumerable<int> genreIds);

    Task<bool> SaveChangesAsync();

    Task UpdateMovieRoles(
        IEnumerable<int> newGenreIds,
        int movieId);

    Task UpdateMovieGenres(
        IEnumerable<int> newGenreIds,
        int movieId);
    
    Task DeleteMovie(int id);

    Task<IEnumerable<Actor>> GetMovieRoles(int movieId);

    Task<IEnumerable<Genre>> GetMovieGenres(int movieId);
    
    Task<Director?> GetDirector(int movieId);

    Task<ContactInformation?> GetContactInformation(int directorId);
}