using System;
using System.Security.Cryptography;
using System.Text;

namespace Template.MinimalApi.NET8.Extensions;

public static class AuthManagerExtensions
{
    public static string GeneratePasswordHash(string password, string salt)
    {
        using var sha256 = SHA256.Create();
        var saltedPassword = $"{password}{salt}";
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));

        return Convert.ToBase64String(bytes);
    }
}