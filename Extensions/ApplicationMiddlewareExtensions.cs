using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerUI;
using Template.MinimalApi.NET8.Data;

namespace Template.MinimalApi.NET8.Extensions;

public static class ApplicationMiddlewareExtensions
{
    public static void UseSwaggerDocumentation(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.DocumentTitle = "Template.MinimalApi.NET8";
            
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Template.MinimalApi.NET8");
            c.RoutePrefix = "swagger";
            
            // Optionally, enable XML documentation support in Swagger UI:
            c.EnableValidator();
            c.DefaultModelRendering(ModelRendering.Example);
            c.EnableFilter();
            c.DocExpansion(DocExpansion.None);
            c.EnableDeepLinking();
            c.DefaultModelExpandDepth(6);
        });
    }

    public static async Task MigrateDatabaseAsync(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices
            .GetRequiredService<IServiceScopeFactory>()
            .CreateScope();

        var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
        var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

        try
        {
            if ((await context?.Database.GetPendingMigrationsAsync()!).Any())
            {
                logger.LogInformation("There are pending migrations. Applying now...");
                await context.Database.MigrateAsync();
                logger.LogInformation("All pending migrations applied successfully");
            }
            else
            {
                logger.LogInformation("No pending migrations found");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating the database");
            throw; // Re-throwing the exception to halt application startup.
        }
    }
}