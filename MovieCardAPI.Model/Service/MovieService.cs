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
            !(await _repository.IsExistingGenres(movie.GenreIds)))
        {
            return null;
        }
        await _repository.CreateMovie(movieEntity, movie.ActorIds, movie.GenreIds);
        await _repository.SaveChangesAsync();
        return _mapper.MapMovieEntityToMovieDTO(movieEntity);
    }

    public async Task<MovieDTO?> UpdateMovie(int id, MovieForUpdateDTO movie)
    {
        if (!(await _repository.IsExistingDirector(movie.DirectorId)) ||
            !(await _repository.IsExistingActors(movie.ActorIds)) ||
            !(await _repository.IsExistingGenres(movie.GenreIds)))
        {
            return null;
        }

        var movieEntity = await _repository.GetMovie(id);
        if (movieEntity == null)
        {
            return null;
        }


        await _repository.UpdateMovieRoles(
            movie.ActorIds,
            movieEntity.Id);

        await _repository.UpdateMovieGenres(
            movie.GenreIds,
            movieEntity.Id);

        movieEntity.Title = movie.Title;
        movieEntity.Rating = movie.Rating;
        movieEntity.TimeStamp = movie.TimeStamp;
        movieEntity.Description = movie.Description;
        movieEntity.DirectorId = movie.DirectorId;

        await _repository.SaveChangesAsync();

        return _mapper.MapMovieEntityToMovieDTO(movieEntity);
    }

    public async Task<bool> DeleteMovie(int id)
    {
        var movieEntity = await _repository.GetMovie(id);
        if (movieEntity == null)
        {
            return false;
        }
        await _repository.DeleteMovie(id);
        return await _repository.SaveChangesAsync();
    }
}
