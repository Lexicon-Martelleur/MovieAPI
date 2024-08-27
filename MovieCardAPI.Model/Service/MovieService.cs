using MovieCardAPI.Model.DTO;
using MovieCardAPI.Model.Repository;
using MovieCardAPI.Model.Utility;

namespace MovieCardAPI.Model.Service;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _repository;

    private readonly IMapper _mapper;

    public MovieService(IMovieRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<MovieDTO?> GetMovie(int id)
    {
        var movieEntity = await _repository.GetMovie(id);
        if (movieEntity == null) {
            return null;
        }
        return _mapper.MapMovieEntityToMovieDTO(movieEntity);
    }

    public async Task<IEnumerable<MovieDTO>> GetMovies()
    {
        var movieEntities = await _repository.GetMovies();
        return _mapper.MapMovieEntitiesToMovieDTOs(movieEntities);
    }

    public async Task<MovieDTO?> CreateMovie(MovieForCreationDTO movie)
    {
        var movieEntity = _mapper.MapMovieForCreationDTOToMovieEntity(movie);
        if (!(await _repository.IsExistingDirector(movie.DirectorId)) ||
            !(await _repository.IsExistingActors(movie.ActorIds)) ||
            !(await _repository.IsExistingGenres(movie.Genres)))
        {
            return null;
        }
        await _repository.CreateMovie(movieEntity, movie.ActorIds, movie.Genres);
        await _repository.SaveChangesAsync();
        return _mapper.MapMovieEntityToMovieDTO(movieEntity);
    }
}
