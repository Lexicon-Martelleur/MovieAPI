using Microsoft.AspNetCore.Mvc;
using MovieCardAPI.Constants;
using MovieCardAPI.Model.DTO;
using MovieCardAPI.Model.Service;

namespace MovieCardAPI.Controllers;

[Route(Router.MOVIE)]
[ApiController]
public class MovieController(
    ILogger<MovieController> logger,
    IMovieService service) : ControllerBase
{
    private readonly ILogger<MovieController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    private readonly IMovieService _service = service ?? throw new ArgumentNullException(nameof(service));

    [HttpGet(Name = "GetMovies")]
    public async Task<ActionResult<IEnumerable<MovieDTO>>> GetMovies()
    {
        var movieDTOs = await _service.GetMovies();
        return Ok(movieDTOs);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MovieDTO>> GetMovie(int id)
    {
        var movieDTO = await _service.GetMovie(id);
        if (movieDTO == null)
        {
            return NotFound(movieDTO);
        }
        return Ok(movieDTO);
    }
}
