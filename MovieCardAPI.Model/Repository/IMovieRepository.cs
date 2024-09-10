using MovieCardAPI.Entities;
using MovieCardAPI.Model.DTO;

namespace MovieCardAPI.Model.Repository;

public interface IMovieRepository : IBaseRepository<Movie>
{
    Task<(IEnumerable<Movie> Movies, PaginationMetaDTO Pagination)> GetMovies(
        PaginationDTO pagination);

    Task<Movie?> GetMovie(int id);

    void CreateMovie(
        Movie movie,
        IEnumerable<int> actorIds,
        IEnumerable<int> genreIds);

    Task DeleteMovie(int id);

    Task<Director?> GeMovieDirector(int movieId);
}