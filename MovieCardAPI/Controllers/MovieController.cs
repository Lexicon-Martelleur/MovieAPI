using Microsoft.AspNetCore.Mvc;
using MovieCardAPI.Constants;
using MovieCardAPI.Model.Service;

namespace MovieCardAPI.Controllers;

[Route(Router.MOVIE)]
[ApiController]
public class MovieController : ControllerBase
{
    private readonly ILogger<MovieController> _logger;

    private readonly IMovieService _service;
    
    public MovieController(
        ILogger<MovieController> logger,
        IMovieService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }


    [HttpGet(Name = "GetMovies")]
    public async Task<ActionResult<IEnumerable<string>>> GetMovies()
    {
        // throw new ArgumentException();
        return NotFound();
        var movieCardDTOs = await _service.GetMovies();
        return Ok(movieCardDTOs);
    }
}
