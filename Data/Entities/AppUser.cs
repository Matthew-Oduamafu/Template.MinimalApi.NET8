using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

#pragma warning disable CS8618

namespace Template.MinimalApi.NET8.Data.Entities;

[Index(nameof(UserName), nameof(Email), IsUnique = true)]
public class AppUser : IdentityUser
{
    [MaxLength(50)] public string FirstName { get; set; }

    [MaxLength(50)] public string LastName { get; set; }
}