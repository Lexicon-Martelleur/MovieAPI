using MovieCardAPI.Model.DTO;

namespace MovieCardAPI.Model.Service;

public interface IMovieService
{
    Task<IEnumerable<MovieCardDTO>> GetMovies();
}