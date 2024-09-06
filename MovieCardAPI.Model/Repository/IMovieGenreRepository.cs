namespace MovieCardAPI.Model.Repository;

public interface IMovieGenreRepository : IBaseRepository
{
    void CreateMovieGenres(int movieId, IEnumerable<int> genreIds);

    void UpdateMovieGenres(IEnumerable<int> newGenreIds, int movieId);

    Task RemoveMovieGenres(int movieId);
}