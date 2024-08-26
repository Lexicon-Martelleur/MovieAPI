using Microsoft.AspNetCore.Mvc;
using MovieCardAPI.Constants;
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
    public async Task<ActionResult<IEnumerable<string>>> GetMovies()
    {
        var movieCardDTOs = await _service.GetMovies();
        return Ok(movieCardDTOs);
    }
}
