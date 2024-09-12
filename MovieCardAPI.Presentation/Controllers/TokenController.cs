using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using MovieCardAPI.Model.DTO;
using MovieCardAPI.Model.Service;
using MovieCardAPI.Presentation.Constants;

namespace MovieCardAPI.Presentation.Controllers;

[Route(AppRouter.TOKEN)]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly IServiceManager _mgr;

    public TokenController(IServiceManager mgr)
    {
        _mgr = mgr;
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<TokenDTO>> RefreshToken(
        [FromBody] TokenDTO token)
    {
        var tokenDto = await _mgr.AuthenticationService.RefreshTokenAsync(token);
        return Ok(tokenDto);
    }
}
