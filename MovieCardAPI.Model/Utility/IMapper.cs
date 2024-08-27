using MovieCardAPI.Entities;
using MovieCardAPI.Model.DTO;

namespace MovieCardAPI.Model.Utility
{
    public interface IMapper
    {
        IEnumerable<MovieDTO> MapMovieEntitiesToMovieDTOs(IEnumerable<Movie> movieEntities);

        MovieDTO MapMovieEntityToMovieDTO(Movie movie);
    }
}