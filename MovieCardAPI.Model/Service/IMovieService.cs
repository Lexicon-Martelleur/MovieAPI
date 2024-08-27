using MovieCardAPI.Entities;
using MovieCardAPI.Model.DTO;

namespace MovieCardAPI.Model.Service;

public interface IMovieService
{
    Task<IEnumerable<MovieDTO>> GetMovies();

    public Task<MovieDTO?> GetMovie(int id);

    public Task<MovieDTO?> CreateMovie(MovieForCreationDTO movie);
    Task<MovieDTO?> UpdateMovie(int id, MovieForUpdateDTO movie);
}