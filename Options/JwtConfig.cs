using System.ComponentModel.DataAnnotations;

#pragma warning disable CS8618

namespace Template.MinimalApi.NET8.Options;

public class JwtConfig
{
    [Required(AllowEmptyStrings = false)]
    public string Issuer { get; set; }
    
    [Required(AllowEmptyStrings = false)]
    public string Audience { get; set; }
    
    [Required(AllowEmptyStrings = false)]
    public string Key { get; set; }
    
    /***
     * In minutes
     */
    [Required]
    [Range(0, Int32.MaxValue)]
    public int AccessTokenExpiration { get; set; }
    
    [Required]
    [Range(0, Int32.MaxValue)]
    public int RefreshTokenExpiration { get; set; }
}