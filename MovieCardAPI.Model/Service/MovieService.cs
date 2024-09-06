using MovieCardAPI.Model.DTO;
using MovieCardAPI.Model.Exeptions;
using MovieCardAPI.Model.Repository;
using MovieCardAPI.Model.Utility;

namespace MovieCardAPI.Model.Service;

public class MovieService : IMovieService
{
    private readonly IUnitOfWork _uow;

    private readonly IMapper _mapper;

    public MovieService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<MovieDTO> GetMovie(int id)
    {
        var movieEntity = await _uow.MovieRepository.GetMovie(id)
            ?? throw new MovieNotFoundException(id);
        var movie = _mapper.MapMovieEntityToMovieDTO(movieEntity);
        return movie;
    }

    public async Task<IEnumerable<MovieDTO>> GetMovies()
    {
        var movieEntities = await _uow.MovieRepository.GetMovies();
        return _mapper.MapMovieEntitiesToMovieDTOs(movieEntities);
    }

    public async Task<MovieDetailsDTO> GetMovieDetails(int id)
    {
        var director = await _uow.MovieRepository.GeMovieDirector(id)
            ?? throw new DirectorNotFoundException(id);
        var contactInformation = await _uow.DirectorRepository
            .GetContactInformation(director.Id)
            ?? throw new ContactInformationNotFoundException(id);
        
        var actors = await _uow.ActorRepository.GetActors(id);
        var genres = await _uow.GenreRepository.GetGenres(id);
        var movie = await _uow.MovieRepository.GetMovie(id)
            ?? throw new MovieNotFoundException(id);
        
        return _mapper.MapMovieEntitiesToMovieDetailsDTO(
            movie,
            actors,
            genres,
            contactInformation,
            director);
    }

    public async Task<MovieDTO> CreateMovie(MovieForCreationDTO movie)
    {
        var movieEntity = _mapper.MapMovieForCreationDTOToMovieEntity(movie);
        if (!(await _uow.DirectorRepository.IsExistingDirector(movie.DirectorId)) ||
            !(await _uow.ActorRepository.IsExistingActors(movie.ActorIds)) ||
            !(await _uow.GenreRepository.IsExistingGenres(movie.GenreIds)))
        {
            throw new ResourceNotFoundException("Multiple resources");
        }

        var isStored = await _uow.MovieRepository.CommitTransaction([
            _uow.MovieRepository.AsAsync(() => {
                _uow.MovieRepository.CreateMovie(movieEntity, movie.ActorIds, movie.GenreIds);
            }),
            _uow.MovieRepository.AsAsync(() => {
                _uow.MovieGenreRepository.CreateMovieGenres(movieEntity.Id, movie.ActorIds);
                _uow.MovieRoleRepository.CreateMovieRoles(movieEntity.Id, movie.GenreIds);
            })
        ]);
        if (!isStored)
        {
            throw new DomainUnableOperationException(
                "Failed to create the movie. Please check the provided data and try again.");
        }

        return _mapper.MapMovieEntityToMovieDTO(movieEntity);
    }

    public async Task<MovieDTO> UpdateMovie(int id, MovieForUpdateDTO movie)
    {
        if (!(await _uow.DirectorRepository.IsExistingDirector(movie.DirectorId)) ||
            !(await _uow.ActorRepository.IsExistingActors(movie.ActorIds)) ||
            !(await _uow.GenreRepository.IsExistingGenres(movie.GenreIds)))
        {
            throw new ResourceNotFoundException("Multiple resources");
        }

        var movieEntity = await _uow.MovieRepository.GetMovie(id)
            ?? throw new MovieNotFoundException(id);
        
        var isStored = await _uow.MovieRepository.CommitTransaction([
            async () => {
                await _uow.MovieRoleRepository.RemoveMovieRoles(movieEntity.Id);
            },
            async () => {
                await _uow.MovieGenreRepository.RemoveMovieGenres(movieEntity.Id);
            },
            _uow.MovieRepository.AsAsync(() => {
                _uow.MovieRoleRepository.UpdateMovieRoles(movie.ActorIds, movieEntity.Id);
                _uow.MovieGenreRepository.UpdateMovieGenres(movie.GenreIds, movieEntity.Id);
            }),
            _uow.MovieRepository.AsAsync(() => {
                movieEntity.Title = movie.Title;
                movieEntity.Rating = movie.Rating;
                movieEntity.TimeStamp = movie.TimeStamp;
                movieEntity.Description = movie.Description;
                movieEntity.DirectorId = movie.DirectorId;
            })
        ]);
        if (!isStored) {
            throw new DomainUnableOperationException(
                "Failed to update the movie. Please check the provided data and try again.");
        }

        return _mapper.MapMovieEntityToMovieDTO(movieEntity);
    }

    public async Task<bool> DeleteMovie(int id)
    {
        var movieEntity = await _uow.MovieRepository.GetMovie(id)
            ?? throw new MovieNotFoundException(id);
        
        await _uow.MovieRepository.DeleteMovie(movieEntity.Id);
        return await _uow.SaveChangesAsync();
    }
}
