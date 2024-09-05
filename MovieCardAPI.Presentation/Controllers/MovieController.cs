using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieCardAPI.Model.DTO;
using MovieCardAPI.Model.Service;
using MovieCardAPI.Presentation.Constants;
using MovieCardAPI.Presentation.Error;
using System.Net;

namespace MovieCardAPI.Presentation.Controllers;

[ApiController]
[Route(Router.MOVIE)]
public class MovieController : ControllerBase
{

    private readonly ILogger<MovieController> _logger;
    private readonly IMovieService _service;

    public MovieController(ILogger<MovieController> logger, IMovieService service)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MovieDTO>>> GetMovies()
    {
        var movieDTOs = await _service.GetMovies();
        return Ok(movieDTOs);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MovieDTO>> GetMovie(
        [FromRoute] int id
    )
    {
        var movieDTO = await _service.GetMovie(id);
        return Ok(movieDTO);
    }

    [HttpGet("{id}/details")]
    public async Task<ActionResult<MovieDetailsDTO>> GetMovieDetails(
        [FromRoute] int id
    )
    {
        var movieDetailsDTO = await _service.GetMovieDetails(id);
        return Ok(movieDetailsDTO);
    }

    [HttpPost(Name = "CreateMovie")]
    public async Task<ActionResult<MovieDTO>> CreateMovie(
        [FromBody] MovieForCreationDTO movie)
    {
        var createdMovie = await _service.CreateMovie(movie);
        return CreatedAtRoute(nameof(CreateMovie), createdMovie);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<MovieDTO>> UpdateMovie(
        [FromRoute] int id,
        [FromBody] MovieForUpdateDTO movie)
    {
        var updatedMovie = await _service.UpdateMovie(id, movie);
        return Ok(updatedMovie);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovie(
        [FromRoute] int id)
    {
        var isDeleted = await _service.DeleteMovie(id);
        if (!isDeleted)
        {
            return NotFound(new ApiError(
               HttpStatusCode.NotFound,
               "Failed to delete the movie. Please check the provided data and try again."
           ));
        }
        return Ok();
    }
}
