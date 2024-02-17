using Template.MinimalApi.NET8.Models;
using Template.MinimalApi.NET8.Models.Dtos;

namespace Template.MinimalApi.NET8.Services.Interfaces;

public interface IAuthManager
{
    Task<IGenericApiResponse<LoginOrRegisterResponseDto>> RegisterUserAsync(AppUserDto user);
    Task<IGenericApiResponse<LoginOrRegisterResponseDto>> LoginAsync(LoginUserDto user);
    Task<string> CreateRefreshTokenAsync();
    Task<IGenericApiResponse<RefreshTokenResponseDto>> VerifyRefreshTokenAsync(LoginOrRegisterResponseDto request);
}