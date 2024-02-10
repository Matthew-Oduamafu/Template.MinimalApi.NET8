using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Template.MinimalApi.NET8.Data.Entities;
using Template.MinimalApi.NET8.Extensions;
using Template.MinimalApi.NET8.Models;
using Template.MinimalApi.NET8.Models.Dtos;
using Template.MinimalApi.NET8.Options;
using Template.MinimalApi.NET8.Repositories.Interfaces;
using Template.MinimalApi.NET8.Services.Interfaces;

namespace Template.MinimalApi.NET8.Services.Providers;

public class AuthManager : IAuthManager
{
    private readonly IAppUserRepository _appUserRepo;
    private readonly ILogger<AuthManager> _logger;
    private readonly JwtConfig _jwtConfig;

    public AuthManager(IAppUserRepository appUserRepo,
        ILogger<AuthManager> logger, IOptionsMonitor<JwtConfig> jwtConfigOpt)
    {
        _appUserRepo = appUserRepo;
        _logger = logger;
        _jwtConfig = jwtConfigOpt.CurrentValue;
    }

    public async Task<IGenericApiResponse<LoginOrRegisterResponseDto>> RegisterUserAsync(AppUserDto user)
    {
        try
        {
            var existingUser = await _appUserRepo.GetByUseNameAndEmailAsync(user.UserName, user.Email);
            if (existingUser != null)
                return GenericApiResponse<LoginOrRegisterResponseDto>.Default.ToBadRequestApiResponse(
                    "User already exists");

            var newUser = new AppUser
            {
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Salt = user.Salt,
                PasswordHash = AuthManagerExtensions.GeneratePasswordHash(user.Password, user.Salt)
            };

            var result = await _appUserRepo.CreateAsync(newUser);
            
            if(!result) return GenericApiResponse<LoginOrRegisterResponseDto>.Default.ToFailedDependenciesApiResponse();

            var response = new LoginOrRegisterResponseDto
            {
                Token = await GenerateToken(newUser)
            };

            return response.ToCreatedApiResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return GenericApiResponse<LoginOrRegisterResponseDto>.Default.ToInternalServerErrorApiResponse();
        }
    }

    public async Task<IGenericApiResponse<LoginOrRegisterResponseDto>> LoginAsync(LoginUserDto user)
    {
        try
        {
            var existingUser = await _appUserRepo.GetByUseNameAsync(user.Username);
            if (existingUser == null)
                return GenericApiResponse<LoginOrRegisterResponseDto>.Default.ToUnAuthorizedApiResponse(
                    "Invalid credentials");

            var passwordHash = AuthManagerExtensions.GeneratePasswordHash(user.Password, existingUser.Salt);
            if (!passwordHash.Equals(existingUser.PasswordHash))
                return GenericApiResponse<LoginOrRegisterResponseDto>.Default.ToUnAuthorizedApiResponse(
                    "Invalid credentials");

            var response = new LoginOrRegisterResponseDto
            {
                Token = await GenerateToken(existingUser)
            };

            return response.ToOkApiResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return GenericApiResponse<LoginOrRegisterResponseDto>.Default.ToInternalServerErrorApiResponse();
        }
    }

    /***
     * Generate token
     */
    private async Task<string> GenerateToken(AppUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtConfig.Key);
        var secret = new SymmetricSecurityKey(key);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, "User"),
            }),
            Expires = DateTime.UtcNow.AddMinutes(_jwtConfig.AccessTokenExpiration),
            SigningCredentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256Signature),
            Issuer = _jwtConfig.Issuer,
            Audience = _jwtConfig.Audience
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        await Task.CompletedTask;
        
        return tokenHandler.WriteToken(token);
    }


    /***
    private async Task<string> GenerateTokenV2(AppUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtConfig.Key);
        var secret = new SymmetricSecurityKey(key);
        var signingCredentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256Signature);

        // var roles = Get roles from user or DB;
        // create claims out of roles
        // get user claims for db or user if any


        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, "User"),
        };

        var token = new JwtSecurityToken(
            _jwtConfig.Issuer,
            _jwtConfig.Audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtConfig.AccessTokenExpiration),
            signingCredentials: signingCredentials
        );

        return tokenHandler.WriteToken(token);
    }
    */
}