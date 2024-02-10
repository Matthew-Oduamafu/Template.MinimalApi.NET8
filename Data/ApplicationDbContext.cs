using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Template.MinimalApi.NET8.Data.Entities;

namespace Template.MinimalApi.NET8.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<AppUser> Users => Set<AppUser>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        builder.Entity<Employee>().ToTable("Employees");
        builder.Entity<AppUser>().ToTable("Users");
    }
}

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        // get environment\
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        
        // build config
        var config = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Template.MinimalApi.NET8"))
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();
        
        // get connection string
        var connectionString = config.GetConnectionString("DbConnection");
        // dbContext builder 
        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
        // use mysql
        builder.UseMySql(connectionString!, ServerVersion.AutoDetect(connectionString));
        // return dbContext
        return new ApplicationDbContext(builder.Options);
    }
}