using MovieCardAPI.Entities;
using MovieCardAPI.Model.DTO;
using MovieCardAPI.Model.ValueObjects;

namespace MovieCardAPI.Model.Utility;

public interface IMapper
{
    IEnumerable<MovieDTO> MapMovieEntitiesToMovieDTOs(IEnumerable<Movie> movieEntities);

    MovieDTO MapMovieEntityToMovieDTO(Movie movie);

    Movie MapMovieForCreationDTOToMovieEntity(
        MovieForCreationDTO movie);

    MovieDetailsDTO MapMovieEntitiesToMovieDetailsDTO(
        Movie movie,
        IEnumerable<Actor> actors,
        IEnumerable<Genre> genres,
        ContactInformation contactInformation,
        Director director);
}
