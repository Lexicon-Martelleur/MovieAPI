using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieCardAPI.Model.DTO;
using MovieCardAPI.Model.Service;
using MovieCardAPI.Presentation.Constants;

namespace MovieCardAPI.Presentation.Controllers;

[Route(AppRouter.AUTH)]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public AuthenticationController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterUser(
        [FromBody] UserRegistrationDTO userForRegistration)
    {
        var result = await _serviceManager.AuthenticationService.RegisterUserAsync(userForRegistration);
        return result.Succeeded ? StatusCode(StatusCodes.Status201Created) : BadRequest(result.Errors);
    }

    [HttpPost("/login")]
    [AllowAnonymous]
    public async Task<IActionResult> Authenticate(
        [FromBody] UserAuthenticationDTO user)
    {
        if (!await _serviceManager.AuthenticationService.ValidateUserAsync(user))
            return Unauthorized();

        TokenDTO tokenDto = await _serviceManager.AuthenticationService.CreateTokenAsync(expireTime: true);
        return Ok(tokenDto);
    }
}