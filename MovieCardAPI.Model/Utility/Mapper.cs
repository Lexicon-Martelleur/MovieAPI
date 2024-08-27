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
}
