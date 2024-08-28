using MovieCardAPI.Entities;
using MovieCardAPI.Model.DTO;

namespace MovieCardAPI.Model.Utility;

public class Mapper : IMapper
{
    public IEnumerable<MovieDTO> MapMovieEntitiesToMovieDTOs(IEnumerable<Movie> movieEntities)
    {
        return movieEntities.Select(MapMovieEntityToMovieDTO);
    }

    public MovieDTO MapMovieEntityToMovieDTO(Movie movie)
    {
        return new MovieDTO(
            movie.Id,
            movie.Title,
            movie.Rating,
            movie.TimeStamp,
            movie.Description
        );
    }

    public Movie MapMovieForCreationDTOToMovieEntity(
        MovieForCreationDTO movie)
    {
        return new Movie()
        {
            Title = movie.Title,
            Rating = movie.Rating,
            TimeStamp = movie.TimeStamp,
            Description = movie.Description,
            DirectorId = movie.DirectorId
        };
    }

    public MovieDetailsDTO MapMovieEntitiesToMovieDetailsDTO(
        Movie movie,
        IEnumerable<Actor> actors,
        IEnumerable<Genre> genres,
        ContactInformation contactInformation,
        Director director)
    {
        return new MovieDetailsDTO(
            movie.Id,
            movie.Title,
            movie.Rating,
            movie.TimeStamp,
            movie.Description,
            MapDirectorEntityToDirectorDTO(director, contactInformation),
            actors.Select(MapActorEntityToActorDTO).ToArray(),
            genres.Select(MapGenreEntityToGenreDTO).ToArray()
        );
    }

    private ActorDTO MapActorEntityToActorDTO(Actor actor)
    {
        return new ActorDTO(
            actor.Id,
            actor.Name,
            actor.DateOfBirth
        );
    }

    private GenreDTO MapGenreEntityToGenreDTO(Genre genre)
    {
        return new GenreDTO(
            genre.Id,
            genre.Name
        );
    }

    private DirectorDTO MapDirectorEntityToDirectorDTO(
        Director director,
        ContactInformation contactInformation)
    {
        return new DirectorDTO(
            director.Id,
            director.Name,
            director.DateOfBirth,
            MapContactInforamtionEntityToGenreDTO(contactInformation)
        );
    }

    private ContactInformationDTO MapContactInforamtionEntityToGenreDTO(
        ContactInformation contactInformation)
    {
        return new ContactInformationDTO(
            contactInformation.Email,
            contactInformation.PhoneNumber
        );
    }
}
