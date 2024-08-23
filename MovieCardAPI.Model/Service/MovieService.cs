using MovieCardAPI.Model.DTO;
using MovieCardAPI.Model.Repository;

namespace MovieCardAPI.Model.Service;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _repository;
    public MovieService(IMovieRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<IEnumerable<MovieCardDTO>> GetMovies()
    {
        var movieEntities = await _repository.GetMovies();
        return movieEntities.Select(item => new MovieCardDTO(
           item.Id,
           item.Title,
           item.Rating,
           item.TimeStamp,
           item.Description
       ));
    }
}
