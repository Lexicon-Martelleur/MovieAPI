using MovieCardAPI.Entities;

namespace MovieCardAPI.Model.Repository;

public interface IMovieRepository
{
    public Task<IEnumerable<Movie>> GetMovies();
}