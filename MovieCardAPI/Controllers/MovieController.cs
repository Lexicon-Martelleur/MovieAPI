using Microsoft.AspNetCore.Mvc;
using MovieCardAPI.Constants;
using System.Text.Json;

namespace MovieCardAPI.Controllers;

[Route(Router.MOVIE)]
[ApiController]
public class MovieController : ControllerBase
{
    private static readonly string[] Movies = new[]
    {
        "movie1"
    };

    private readonly ILogger<MovieController> _logger;

    public MovieController(ILogger<MovieController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetMovies")]
    public async Task<ActionResult<IEnumerable<string>>> GetMovies()
    {
        return Ok(Movies ?? []);
    }
}
