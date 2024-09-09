using MovieCardAPI.Model.DTO;

namespace MovieCardAPI.Model.Service;

public interface IGenreService
{
    Task<IEnumerable<GenreDTO>> GetGenres();
}
