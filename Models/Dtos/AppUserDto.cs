using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

#pragma warning disable CS8618

namespace Template.MinimalApi.NET8.Models.Dtos;

public class AppUserDto
{
    public string UserName { get; set; }

    [Required]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$",
        ErrorMessage =
            "Password must be at least 6 characters and contain at least 1 uppercase letter, 1 lowercase letter, 1 number, and 1 special character.")]
    public string Password { get; set; }

    [JsonIgnore] public string Salt { get; set; } = Guid.NewGuid().ToString();

    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public string Email { get; set; }

    [MaxLength(50)] public string FirstName { get; set; }

    [MaxLength(50)] public string LastName { get; set; }
    [MaxLength(20)]
    public string PhoneNumber { get; set; }
}

public class LoginUserDto
{
    public string Username { get; set; }

    [Required] public string Password { get; set; }
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