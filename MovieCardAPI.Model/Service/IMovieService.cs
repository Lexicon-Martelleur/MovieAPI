using MovieCardAPI.Entities;
using MovieCardAPI.Model.DTO;

namespace MovieCardAPI.Model.Service;

public interface IMovieService
{
    Task<IEnumerable<MovieDTO>> GetMovies();

    public Task<MovieDTO?> GetMovie(int id);
}