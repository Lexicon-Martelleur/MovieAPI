using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MovieCardAPI.Constants;
using MovieCardAPI.Entities;
using MovieCardAPI.Model.Configuration;
using MovieCardAPI.Model.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MovieCardAPI.Model.Service;

public class AuthenticationService : IAuthenticationService
{
    private readonly IJWTConfiguration _jwtConfiguration;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private ApplicationUser? _user;

    public AuthenticationService(
        IJWTConfiguration jwtConfiguration,
        IMapper mapper,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration)
    {
        _jwtConfiguration = jwtConfiguration;
        _mapper = mapper;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    public async Task<IdentityResult> RegisterUserAsync(UserRegistrationDTO userDTO)
    {
        ArgumentNullException.ThrowIfNull(userDTO, nameof(userDTO));

        var roleExists = await _roleManager.RoleExistsAsync(userDTO.Position);
        if (!roleExists)
        {
            return IdentityResult.Failed(new IdentityError { Description = "Role does not exist" });
        }

        var user = _mapper.Map<ApplicationUser>(userDTO);
        var result = await _userManager.CreateAsync(user, userDTO.Password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, userDTO.Position);
        }
        return result;
    }

    /**
     * TODO Return user and then use it in Create token
     * instead of a user field.
     */
    public async Task<bool> ValidateUserAsync(UserAuthenticationDTO userDTO)
    {
        ArgumentNullException.ThrowIfNull(userDTO, nameof(userDTO));

        _user = await _userManager.FindByNameAsync(userDTO.UserName!);
        var validUserPwd = _user != null && 
            await _userManager.CheckPasswordAsync(_user, userDTO.Password!);

        return _user != null && await _userManager.CheckPasswordAsync(_user, userDTO.Password!);
    }

    public async Task<TokenDTO> CreateTokenAsync(bool expireTime)
    {
        SigningCredentials signing = GetSigningCredentials();
        IEnumerable<Claim> claims = await GetClaimsAsync();
        JwtSecurityToken tokenOptions = GenerateTokenOptions(signing, claims);

        ArgumentNullException.ThrowIfNull(_user, nameof(_user));

        _user.RefreshToken = GenerateRefreshToken();

        if (expireTime)
        {
            _user.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(
                AppConfig.RefreshTokenExpireTime);
        }

        // TODO validate identityResult!
        var identityResult = await _userManager.UpdateAsync(_user);
        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return new TokenDTO(accessToken, _user.RefreshToken);
    }

    private SigningCredentials GetSigningCredentials()
    {
        var secretKey = AppConfig.GetSecretKey(_configuration);
        ArgumentNullException.ThrowIfNull(secretKey, nameof(secretKey));

        var key = Encoding.UTF8.GetBytes(secretKey);
        var secret = new SymmetricSecurityKey(key);

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);

    }

    private async Task<IEnumerable<Claim>> GetClaimsAsync()
    {
        ArgumentNullException.ThrowIfNull(_user);

        var claims = new List<Claim>()
        {
            new(ClaimTypes.Name, _user.UserName!),
            new(ClaimTypes.NameIdentifier, _user.Id),
        };

        var roles = await _userManager.GetRolesAsync(_user);

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signing, IEnumerable<Claim> claims)
    {
        var expireTime = Convert.ToDouble(_jwtConfiguration.Expires);
        var tokenOptions = new JwtSecurityToken(
            issuer: _jwtConfiguration.Issuer,
            audience: _jwtConfiguration.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(expireTime),
            signingCredentials: signing);

        return tokenOptions;
    }

    private string GenerateRefreshToken()
    {
        var byteArray = new byte[32];
        using var generator = RandomNumberGenerator.Create();
        generator.GetBytes(byteArray);
        return Convert.ToBase64String(byteArray);
    }

    public async Task<TokenDTO> RefreshTokenAsync(TokenDTO token)
    {
        ClaimsPrincipal principal = GetPrincipalFromExpiredToken(token.AccessToken);

        ApplicationUser? user = await _userManager.FindByNameAsync(principal.Identity?.Name!);

        var invalidToken = user == null ||
            user.RefreshToken != token.RefreshToken ||
            user.RefreshTokenExpireTime <= DateTime.Now;

        //TODO: Should be handled with middle-ware and custom exception class
        if (invalidToken) { throw new ArgumentException("Invalid TokenDTO"); }

        _user = user;

        return await CreateTokenAsync(expireTime: false);
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken)
    {
        var secretKey = AppConfig.GetSecretKey(_configuration);
        ArgumentNullException.ThrowIfNull(nameof(secretKey));

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtConfiguration.Issuer,
            ValidAudience = _jwtConfiguration.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        ClaimsPrincipal principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }
}
