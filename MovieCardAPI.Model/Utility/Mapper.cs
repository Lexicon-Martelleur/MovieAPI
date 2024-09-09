using MovieCardAPI.Entities;
using MovieCardAPI.Model.DTO;
using MovieCardAPI.Model.Validation;

namespace MovieCardAPI.Model.Utility;

public class Mapper : IMapper
{
    public IEnumerable<MovieDTO> MapMovieEntitiesToMovieDTOs(IEnumerable<Movie> movieEntities)
    {
        return movieEntities.Select(MapMovieEntityToMovieDTO);
    }

    public MovieDTO MapMovieEntityToMovieDTO(Movie movie)
    {
        return new MovieDTO
        {
            Id = movie.Id,
            Title = movie.Title,
            Rating = movie.Rating,
            TimeStamp = movie.TimeStamp,
            Description = movie.Description
        };
    }

    public MovieDTO GetMovieDTO()
    {
        return new MovieDTO
        {
            Id = 1,
            Title = "Title",
            Rating = 3,
            TimeStamp = 123,
            Description = "des"
        };
    }

    public Movie MapMovieForCreationDTOToMovieEntity(
        MovieForCreationDTO movie)
    {
        return new Movie
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
        return new MovieDetailsDTO {
            Id = movie.Id,
            Title = movie.Title,
            Rating = movie.Rating,
            TimeStamp = movie.TimeStamp,
            Description = movie.Description,
            Director = MapDirectorEntityToDirectorDTO(director, contactInformation),
            Actors = actors.Select(MapActorEntityToActorDTO).ToArray(),
            Genres = genres.Select(MapGenreEntityToGenreDTO).ToArray()
        };
    }

    public IEnumerable<ActorDTO> MapActorEntitiesToActorDTOs(
        IEnumerable<Actor> actors)
    {
        return actors.Select(MapActorEntityToActorDTO);
    }

    public ActorDTO MapActorEntityToActorDTO(Actor actor)
    {
        return ValidationService.ValidateInstance(new ActorDTO
        {
            Id = actor.Id,
            Name = actor.Name,
            DateOfBirth = actor.DateOfBirth
        });
    }

    public IEnumerable<DirectorDTO> MapDirectorEntitiesToDirectorDTOs(
        IEnumerable<Director> directors)
    {
        return directors.Select(MapDirectorEntityToDirectorDTO);
    }

    public DirectorDTO MapDirectorEntityToDirectorDTO(
        Director director)
    {
        return ValidationService.ValidateInstance(new DirectorDTO
        {
            Id = director.Id,
            Name = director.Name,
            DateOfBirth = director.DateOfBirth
        });
    }

    public DirectorDTO MapDirectorEntityToDirectorDTO(
        Director director,
        ContactInformation contactInformation)
    {
        return ValidationService.ValidateInstance(new DirectorDTO
        {
            Id = director.Id,
            Name = director.Name,
            DateOfBirth = director.DateOfBirth,
            ContactInformation = MapContactInforamtionEntityToGenreDTO(contactInformation)
        });
    }

    private ContactInformationDTO MapContactInforamtionEntityToGenreDTO(
        ContactInformation contactInformation)
    {
        return ValidationService.ValidateInstance(new ContactInformationDTO
        {
            Email = contactInformation.Email,
            PhoneNumber = contactInformation.PhoneNumber
        });
    }

    public GenreDTO MapGenreEntityToGenreDTO(Genre genre)
    {
        return new GenreDTO(
            genre.Id,
            genre.Name
        );
    }
}
