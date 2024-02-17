using System.Reflection;
using System.Text;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Template.MinimalApi.NET8.Data;
using Template.MinimalApi.NET8.Data.Entities;
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
        services.AddScoped<IAppUserRepository, AppUserRepository>();
        services.AddScoped<IPgRepository, PgRepository>();
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<ILinkService, LinkService>();
        services.AddScoped<IAuthManager, AuthManager>();
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
            });
        });
        
        services.AddIdentityCore<AppUser>()
            .AddRoles<IdentityRole>()
            .AddTokenProvider<DataProtectorTokenProvider<AppUser>>("Template.MinimalApi.NET8")
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        
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
    
    public static IServiceCollection AddJwtConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<JwtConfig>()
            .BindConfiguration(nameof(JwtConfig))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        Action<JwtConfig> jwtConfigAction = jwtConfigOpt =>
            configuration.GetSection(nameof(JwtConfig)).Bind(jwtConfigOpt);
        var jwtConfig = new JwtConfig();
        jwtConfigAction.Invoke(jwtConfig);
        
        
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtConfig.Issuer,
                ValidAudience = jwtConfig.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key)),
                ClockSkew = TimeSpan.Zero,
                
            };
    
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    {
                        context.Response.Headers.Append("Token-Expired", "true");
                    }
                    return Task.CompletedTask;
                }
            };
    
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.IncludeErrorDetails = true;
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
                Title = "Minimal API .NET 8 Template",
                Version = "v1",
                Description = "A minimal API template for .NET 8",
                Contact = new OpenApiContact
                {
                    Email = "mattoduamafu@gmail.com",
                    Extensions = { },
                    Name = "Matthew",
                    Url = new Uri($"https://www.linkedin.com/in/matthew-oduamafu-42a1551a7/")
                },
                License = new OpenApiLicense
                {
                    Name = "License",
                    Url = new Uri($"https://github.com/")
                },
                Extensions = { },
                TermsOfService = new Uri($"https://github.com/Matthew-Oduamafu")
            });

            c.EnableAnnotations();
            
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\nEnter 'Bearer' [space] an then your token in the next input below.\r\n\r\nExample: 'Bearer 1234etetrf'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {{
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme ="oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
            
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
}