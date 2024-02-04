using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerUI;
using Template.MinimalApi.NET8.CustomMiddleware;
using Template.MinimalApi.NET8.Data;

namespace Template.MinimalApi.NET8.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app, ILogger logger)
    {
        return app.UseMiddleware<ExceptionMiddleware>(logger);
    }
}