using Microsoft.AspNetCore.Identity;
using MovieCardAPI.Model.DTO;

namespace MovieCardAPI.Model.Service;
public interface IAuthenticationService
{
    Task<TokenDTO> CreateTokenAsync(bool expireTime);
    Task<TokenDTO> RefreshTokenAsync(TokenDTO token);
    Task<IdentityResult> RegisterUserAsync(UserRegistrationDTO userForRegistration);
    Task<bool> ValidateUserAsync(UserAuthenticationDTO user);

}
