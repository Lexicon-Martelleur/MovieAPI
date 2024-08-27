using Microsoft.AspNetCore.Mvc;
using MovieCardAPI.Constants;
using MovieCardAPI.Error;
using MovieCardAPI.Model.DTO;
using MovieCardAPI.Model.Service;
using System.Net;

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
            return NotFound();
        }
        return Ok(movieDTO);
    }

    [HttpPost(Name = "CreateMovie")]
    public async Task<ActionResult<MovieDTO>> CreateMovie(
        [FromBody] MovieForCreationDTO movie)
    {
        var createdMovie = await _service.CreateMovie(movie);
        if (createdMovie == null)
        {
            return BadRequest(new ApiError(
                HttpStatusCode.BadRequest,
                "Failed to create the movie. Please check the provided data and try again."
            ));
        }
        return CreatedAtRoute("CreateMovie", createdMovie);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<MovieDTO>> UpdateMovie(
        int id,
        [FromBody] MovieForUpdateDTO movie)
    {
        throw new NotImplementedException("_");
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<MovieDTO>> DeleteMovie(int id)
    {
        throw new NotImplementedException("_");
    }
}
