using System.Text.Json;
using Template.MinimalApi.NET8.Models;

namespace Template.MinimalApi.NET8.Extensions;

public static class BaseActionResult
{
    public static IResult ToActionResult<T>(this IGenericApiResponse<T> response)
    {
        var json = JsonSerializer.Serialize(response);
        
        return response.Code switch
        {
            StatusCodes.Status200OK => Results.Ok(response),
            
            StatusCodes.Status202Accepted => Results.Content(json, contentType: "application/json",
                statusCode: StatusCodes.Status202Accepted),
            
            StatusCodes.Status201Created => Results.Content(json, contentType: "application/json",
                statusCode: StatusCodes.Status201Created),
            
            StatusCodes.Status204NoContent => Results.NoContent(),
            
            StatusCodes.Status400BadRequest => Results.BadRequest(response),
            
            StatusCodes.Status401Unauthorized => Results.Content(json, contentType: "application/json",
                statusCode: StatusCodes.Status401Unauthorized),
            
            StatusCodes.Status403Forbidden => Results.Forbid(),
            
            StatusCodes.Status404NotFound => Results.NotFound(response),
            
            StatusCodes.Status424FailedDependency => Results.Content(json, contentType: "application/json",
                statusCode: StatusCodes.Status424FailedDependency),
            
            StatusCodes.Status500InternalServerError => Results.Content(json, contentType: "application/json",
                statusCode: StatusCodes.Status500InternalServerError),
            
            _ => Results.Problem(json, statusCode: StatusCodes.Status500InternalServerError)
        };
    }
}