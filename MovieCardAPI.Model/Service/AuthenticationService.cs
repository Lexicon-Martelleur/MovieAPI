using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MovieCardAPI.Constants;
using MovieCardAPI.Entities;
using MovieCardAPI.Model.Configuration;
using MovieCardAPI.Model.DTO;
using MovieCardAPI.Model.Utility;
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
        this._jwtConfiguration = jwtConfiguration;
        this._mapper = mapper;
        this._userManager = userManager;
        this._roleManager = roleManager;
        this._configuration = configuration;
    }

    public async Task<TokenDTO> CreateTokenAsync(bool expireTime)
    {
        //signingCredentials signing = GetSigningCredentials();
        //IEnumerable<Claim> claims = await GetClaimsAsync();
        //JwtSecurityToken tokenOptions = GenerateTokenOptions(signing, claims);

        //ArgumentNullException.ThrowIfNull(user, nameof(user));

        //user.RefreshToken = GenerateRefreshToken();

        //if (expireTime)
        //    user.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(2);

        //var res = await userManager.UpdateAsync(user); //ToDo validate res!
        //var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        //return new TokenDto(accessToken, user.RefreshToken);
        throw new NotImplementedException();
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signing, IEnumerable<Claim> claims)
    {
        //var jwtSettings = configuration.GetSection("JwtSettings");

        //var tokenOptions = new JwtSecurityToken(
        //    issuer: jwtConfiguration.Issuer,
        //    audience: jwtConfiguration.Audience,
        //    claims: claims,
        //    expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["Expires"])),
        //    signingCredentials: signing);

        //return tokenOptions;
        throw new NotImplementedException();
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

    private SigningCredentials GetSigningCredentials()
    {
        var secretKey = AppConfig.GetSecretKey(_configuration);
        ArgumentNullException.ThrowIfNull(secretKey, nameof(secretKey));

        var key = Encoding.UTF8.GetBytes(secretKey);
        var secret = new SymmetricSecurityKey(key);

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);

    }

    public async Task<IdentityResult> RegisterUserAsync(UserRegistrationDTO userDTO)
    {
        ArgumentNullException.ThrowIfNull(userDTO, nameof(userDTO));

        var roleExists = await _roleManager.RoleExistsAsync(userDTO.Role!);
        if (!roleExists)
        {
            return IdentityResult.Failed(new IdentityError { Description = "Role does not exist" });
        }

        var user = _mapper.Map<ApplicationUser>(userDTO);

        var result = await _userManager.CreateAsync(user, userDTO.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, userDTO.Role!);
        }

        return result;
    }

    public async Task<bool> ValidateUserAsync(UserAuthenticationDTO userDTO)
    {
        ArgumentNullException.ThrowIfNull(userDTO, nameof(userDTO));

        _user = await _userManager.FindByNameAsync(userDTO.UserName!);

        return _user != null && await _userManager.CheckPasswordAsync(_user, userDTO.Password!);

    }

    public async Task<TokenDTO> RefreshTokenAsync(TokenDTO token)
    {
        ClaimsPrincipal principal = GetPrincipalFromExpiredToken(token.AccessToken);

        ApplicationUser? user = await _userManager.FindByNameAsync(principal.Identity?.Name!);

        var invalitToken = user == null ||
            user.RefreshToken != token.RefreshToken ||
            user.RefreshTokenExpireTime <= DateTime.Now;

        //TODO: Should be handled with middle-ware and custom exception class
        if (invalitToken) { throw new ArgumentException("Invalid TokenDTO"); }

        _user = user;

        return await CreateTokenAsync(expireTime: false);
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken)
    {
        var jwtSettings = _configuration.GetSection(JWTConfiguration.Section);

        var secretKey = AppConfig.GetSecretKey(_configuration);
        ArgumentNullException.ThrowIfNull(nameof(secretKey));

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
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
