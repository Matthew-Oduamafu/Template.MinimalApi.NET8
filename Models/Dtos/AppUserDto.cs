using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

#pragma warning disable CS8618

namespace Template.MinimalApi.NET8.Models.Dtos;

public class AppUserDto
{
    [JsonIgnore] public string UserName => Email;
    public string Password { get; set; }
    
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
}

public class LoginUserDto
{
    [Required(AllowEmptyStrings = false)]
    public string Username { get; set; }

    [Required(AllowEmptyStrings = false)] 
    public string Password { get; set; }
}

public class LoginOrRegisterResponseDto
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}

public class RefreshTokenResponseDto
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}