using MovieCardAPI.Entities;
using MovieCardAPI.Model.DTO;
using MovieCardAPI.Model.Validation;

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

    IEnumerable<ActorDTO> MapActorEntitiesToActorDTOs(
        IEnumerable<Actor> actors);

    ActorDTO MapActorEntityToActorDTO(Actor actor);

    IEnumerable<DirectorDTO> MapDirectorEntitiesToDirectorDTOs(
        IEnumerable<Director> directors);

    DirectorDTO MapDirectorEntityToDirectorDTO(
        Director director,
        ContactInformation contactInformation);

    IEnumerable<GenreDTO> MapGenreEntitiesToGenreDTOs(
        IEnumerable<Genre> genres);

    GenreDTO MapGenreEntityToGenreDTO(Genre genre);
}
