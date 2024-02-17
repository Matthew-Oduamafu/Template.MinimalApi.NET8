using FluentValidation;

namespace Template.MinimalApi.NET8.CustomFilters;

public class ValidationFilter<T> : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var validator = context.HttpContext.RequestServices.GetRequiredService<IValidator<T>>();
        if (validator is not null)
        {
            var entity = context.Arguments
                .OfType<T>()
                .FirstOrDefault(a => a?.GetType() == typeof(T));
            if (entity is not null)
            {
                var validation = await validator.ValidateAsync(entity);
                if (validation.IsValid)
                {
                    return await next(context);
                }
                return Results.ValidationProblem(validation.ToDictionary());
            }

            return Results.Problem("Could not find type to validate");
        }
        return await next(context);
    }
}