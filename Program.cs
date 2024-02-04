using Template.MinimalApi.NET8;
using Template.MinimalApi.NET8.Extensions;
using Template.MinimalApi.NET8.Extensions.EndpointsExtensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDatabaseConfiguration(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllerConfiguration();

builder.Services.AddCors(options => options
    .AddPolicy(CommonConstants.CorsPolicyName, policy => policy
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerDocumentation();

var app = builder.Build();

{
    var logger = app.Logger;
    await app.MigrateDatabaseAsync();
// Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
        app.UseDeveloperExceptionPage();
    else
        app.UseCustomExceptionHandler(logger);

    app.UseSwaggerDocumentation();

    app.UseCors(CommonConstants.CorsPolicyName);
    app.UseRouting();
    app.UseHttpsRedirection();
    
    app.MapEmployeeEndpoints();

    await app.RunAsync();
}