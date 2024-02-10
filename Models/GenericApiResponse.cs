namespace Template.MinimalApi.NET8.Models;

public interface IGenericApiResponse<out T>
{
    public string Message { get; }
    public int Code { get; }
    public T? Data { get; }
    public IEnumerable<ErrorResponse>? Errors { get; }
    public static T? Default { get; } = default;
}

public sealed record GenericApiResponse<T>(string Message, int Code, T? Data = default,
    IEnumerable<ErrorResponse>? Errors = default) : IGenericApiResponse<T>
{
    public static T? Default = default;
}

public sealed record ErrorResponse(string Field, string ErrorMessage);

public static class GenericApiResponseExtensions
{
    public static GenericApiResponse<T> ToOkApiResponse<T>(this T data, string message = "Success")
    {
        return ToApiResponse(data, message, StatusCodes.Status200OK);
    }

    public static GenericApiResponse<T> ToCreatedApiResponse<T>(this T data, string message = "Created")
    {
        return ToApiResponse(data, message, StatusCodes.Status201Created);
    }

    public static GenericApiResponse<T> ToAcceptedApiResponse<T>(this T data, string message = "Accepted")
    {
        return ToApiResponse(data, message, StatusCodes.Status202Accepted);
    }

    public static GenericApiResponse<T> ToNotFoundApiResponse<T>(this T? data, string message = "Not Found")
#pragma warning disable CS8604 // Possible null reference argument.
        => ToApiResponse<T>(default, message, StatusCodes.Status404NotFound);

    public static GenericApiResponse<T> ToFailedDependenciesApiResponse<T>(this T? data, string message = "Failed Dependencies")
#pragma warning disable CS8604 // Possible null reference argument.
        => ToApiResponse<T>(default, message, StatusCodes.Status424FailedDependency);

    public static GenericApiResponse<T> ToBadRequestApiResponse<T>(this T? data,
        string message = "Request error. Please verify your data and resend")
    {
        return ToApiResponse<T>(default, message, StatusCodes.Status400BadRequest);
    }

    public static GenericApiResponse<T> ToUnAuthorizedApiResponse<T>(this T? data, string message = "UnAuthorized")
    {
        return ToApiResponse<T>(default, message, StatusCodes.Status401Unauthorized);
    }

    public static GenericApiResponse<T> ToInternalServerErrorApiResponse<T>(this T? data,
        string message = "Oh no! Something went wrong")
    {
        return ToApiResponse<T>(default, message, StatusCodes.Status500InternalServerError);
    }

    private static GenericApiResponse<T> ToApiResponse<T>(this T data, string message, int code,
        IEnumerable<ErrorResponse>? errors = null)

    {
        return new GenericApiResponse<T>(message, code, data, errors);
    }
}