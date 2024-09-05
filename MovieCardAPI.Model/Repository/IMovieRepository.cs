using MovieCardAPI.Entities;

namespace MovieCardAPI.Model.Repository;

public interface IMovieRepository
{
    Task<IEnumerable<Movie>> GetMovies();

    Task<Movie?> GetMovie(int id);

    Task<bool> IsExistingDirector(int id);

    Task<bool> IsExistingActors(IEnumerable<int> ids);

    Task<bool> IsExistingGenres(IEnumerable<int> ids);

    void CreateMovie(
        Movie movie,
        IEnumerable<int> actorIds,
        IEnumerable<int> genreIds);

    void CreateMovieRoles(Movie movie, IEnumerable<int> actorIds);

    void CreateMovieGenres(Movie movie, IEnumerable<int> genreIds);

    Task RemoveMovieRoles(IEnumerable<int> newGenreIds, int movieId);

    void UpdateMovieRoles(IEnumerable<int> newActorIds, int movieId);

    Task RemoveMovieGenres(IEnumerable<int> newGenreIds, int movieId);

    void UpdateMovieGenres(IEnumerable<int> newGenreIds, int movieId);
    
    Task DeleteMovie(int id);

    Task<IEnumerable<Actor>> GetMovieRoles(int movieId);

    Task<IEnumerable<Genre>> GetMovieGenres(int movieId);
    
    Task<Director?> GetDirector(int movieId);

    Task<ContactInformation?> GetContactInformation(int directorId);
}