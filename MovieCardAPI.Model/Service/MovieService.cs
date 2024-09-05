using MovieCardAPI.Model.DTO;
using MovieCardAPI.Model.Repository;
using MovieCardAPI.Model.Utility;

namespace MovieCardAPI.Model.Service;

public class MovieService : IMovieService
{
    // private readonly IMovieRepository _repository;
    private readonly IUnitOfWork _uow;

    private readonly IMapper _mapper;

    public MovieService(IUnitOfWork uow, IMapper mapper)
    {
        // _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<MovieDTO?> GetMovie(int id)
    {
        var movieEntity = await _uow.MovieRepository.GetMovie(id);
        if (movieEntity == null) {
            return null;
        }
        return _mapper.MapMovieEntityToMovieDTO(movieEntity);
    }

    public async Task<IEnumerable<MovieDTO>> GetMovies()
    {
        var movieEntities = await _uow.MovieRepository.GetMovies();
        return _mapper.MapMovieEntitiesToMovieDTOs(movieEntities);
    }

    public async Task<MovieDetailsDTO?> GetMovieDetails(int id)
    {
        var director = await _uow.MovieRepository.GetDirector(id);
        if (director == null) { return null; }


        var contactInformation = await _uow.MovieRepository.GetContactInformation(director.Id);
        if (contactInformation == null) { return null; }

        var actors = await _uow.MovieRepository.GetMovieRoles(id);
        var genres = await _uow.MovieRepository.GetMovieGenres(id);
        var movie = await _uow.MovieRepository.GetMovie(id);

        if (movie == null) { return null; }
        
        return _mapper.MapMovieEntitiesToMovieDetailsDTO(
            movie,
            actors,
            genres,
            contactInformation,
            director);
    }

    public async Task<MovieDTO?> CreateMovie(MovieForCreationDTO movie)
    {
        var movieEntity = _mapper.MapMovieForCreationDTOToMovieEntity(movie);
        if (!(await _uow.MovieRepository.IsExistingDirector(movie.DirectorId)) ||
            !(await _uow.MovieRepository.IsExistingActors(movie.ActorIds)) ||
            !(await _uow.MovieRepository.IsExistingGenres(movie.GenreIds)))
        {
            return null;
        }
        await _uow.MovieRepository.CreateMovie(movieEntity, movie.ActorIds, movie.GenreIds);
        await _uow.SaveChangesAsync();
        return _mapper.MapMovieEntityToMovieDTO(movieEntity);
    }

    public async Task<MovieDTO?> UpdateMovie(int id, MovieForUpdateDTO movie)
    {
        if (!(await _uow.MovieRepository.IsExistingDirector(movie.DirectorId)) ||
            !(await _uow.MovieRepository.IsExistingActors(movie.ActorIds)) ||
            !(await _uow.MovieRepository.IsExistingGenres(movie.GenreIds)))
        {
            return null;
        }

        var movieEntity = await _uow.MovieRepository.GetMovie(id);
        if (movieEntity == null)
        {
            return null;
        }


        await _uow.MovieRepository.UpdateMovieRoles(
            movie.ActorIds,
            movieEntity.Id);

        await _uow.MovieRepository.UpdateMovieGenres(
            movie.GenreIds,
            movieEntity.Id);

        movieEntity.Title = movie.Title;
        movieEntity.Rating = movie.Rating;
        movieEntity.TimeStamp = movie.TimeStamp;
        movieEntity.Description = movie.Description;
        movieEntity.DirectorId = movie.DirectorId;

        await _uow.SaveChangesAsync();

        return _mapper.MapMovieEntityToMovieDTO(movieEntity);
    }

    public async Task<bool> DeleteMovie(int id)
    {
        var movieEntity = await _uow.MovieRepository.GetMovie(id);
        if (movieEntity == null)
        {
            return false;
        }
        await _uow.MovieRepository.DeleteMovie(id);
        return await _uow.SaveChangesAsync();
    }
}
