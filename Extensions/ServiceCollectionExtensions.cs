using System.Reflection;
using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Template.MinimalApi.NET8.Data;
using Template.MinimalApi.NET8.Options;
using Template.MinimalApi.NET8.Repositories.Interfaces;
using Template.MinimalApi.NET8.Repositories.Providers;
using Template.MinimalApi.NET8.Services.Interfaces;
using Template.MinimalApi.NET8.Services.Providers;

namespace Template.MinimalApi.NET8.Extensions;

#pragma warning disable CS1591
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IPgRepository, PgRepository>();
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<ILinkService, LinkService>();
        return services;
    }

    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<DatabaseConfig>()
            .BindConfiguration(nameof(DatabaseConfig))
            .Configure(c => { c.DbConnectionString = configuration.GetConnectionString("DbConnection")!; })
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            var dbConfig = serviceProvider.GetRequiredService<IOptions<DatabaseConfig>>().Value;
            options.UseMySql(dbConfig.DbConnectionString, ServerVersion.AutoDetect(dbConfig.DbConnectionString), builder =>
            {
                if (dbConfig.EnableRetryOnFailure)
                {
                    builder.EnableRetryOnFailure(dbConfig.MaxRetryCount,
                        TimeSpan.FromMilliseconds(dbConfig.MaxRetryDelay),
                        dbConfig.ErrorNumbersToAdd);
                }

                builder.CommandTimeout(dbConfig.CommandTimeout);
                // builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });
        });
        return services;
    }

    public static IServiceCollection AddControllerConfiguration(this IServiceCollection services)
    {
        services.Configure<RouteOptions>(options =>
        {
            options.LowercaseUrls = true;
            options.LowercaseQueryStrings = true;
        });
        
        return services;
    }

    public static void AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.CustomSchemaIds(type => type.FullName ?? type.Name);
            
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "API Title",
                Version = "v1",
                Description = "API description"
            });

            c.EnableAnnotations();
            
            // Optionally, add XML comments:
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });
    }

    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        });
    }
    // public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger logger)
    // {
    //     app.UseMiddleware<ExceptionMiddleware>(logger);
    // }
}