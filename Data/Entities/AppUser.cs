using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

#pragma warning disable CS8618

namespace Template.MinimalApi.NET8.Data.Entities;

[Index(nameof(UserName), nameof(Email), IsUnique = true)]
public class AppUser : BaseEntity
{
    [Required]
    [MaxLength(50)]
    public string UserName { get; set; }

    [Required] public string PasswordHash { get; set; }

    [Required] public string Salt { get; set; }

    [Required]
    [MaxLength(100)]
    public string Email { get; set; }

    [MaxLength(50)] public string FirstName { get; set; }

    [MaxLength(50)] public string LastName { get; set; }
}