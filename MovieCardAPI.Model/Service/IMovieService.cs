using MovieCardAPI.Model.DTO;

namespace MovieCardAPI.Model.Service;

public interface IMovieService
{
    Task<IEnumerable<MovieDTO>> GetMovies();

    Task<MovieDTO> GetMovie(int id);

    Task<MovieDTO> CreateMovie(MovieForCreationDTO movie);
    
    Task<MovieDTO> UpdateMovie(int id, MovieForUpdateDTO movie);
    
    Task<bool> DeleteMovie(int id);
    
    Task<MovieDetailsDTO> GetMovieDetails(int id);
}