using MovieCardAPI.Entities;
using MovieCardAPI.Model.DTO;
using MovieCardAPI.Model.ValueObjects;

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
}
