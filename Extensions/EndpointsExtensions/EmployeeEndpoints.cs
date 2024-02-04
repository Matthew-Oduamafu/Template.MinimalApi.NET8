using Microsoft.AspNetCore.Mvc;
using Template.MinimalApi.NET8.Models;
using Template.MinimalApi.NET8.Models.Dtos;
using Template.MinimalApi.NET8.Services.Interfaces;

namespace Template.MinimalApi.NET8.Extensions.EndpointsExtensions;

public static class EmployeeEndpoints
{
    public static void MapEmployeeEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/employees")
            .WithTags("Employees");

        group.MapGet("",
                async ([FromServices] IEmployeeService employeeService, [FromQuery] int page,
                    [FromQuery] int pageSize) =>
                {
                    var filter = new BaseFilter { Page = page, PageSize = pageSize };
                    var response = await employeeService.GetEmployees(filter);
                    return TypedResults.Json(response);
                })
            .WithName("GetEmployees")
            .Produces<PagedList<EmployeeResponse>>()
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Get all employees with pagination")
            .WithDescription("Some description here<br/>Next line description here")
            .WithOpenApi();

        // post endpoint
        group.MapPost("", async ([FromServices] IEmployeeService employeeService, [FromBody] EmployeeRequest request) =>
            {
                var response = await employeeService.AddEmployeeAsync(request);
                return TypedResults.Created($"/api/employees/{response?.Data?.Id}", response);
            }).WithName("CreateEmployee")
            .Produces<EmployeeResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithOpenApi();

        // Get endpoint
        group.MapGet("{id}", async ([FromServices] IEmployeeService employeeService, [FromRoute] string id) =>
            {
                var response = await employeeService.GetEmployeeByIdAsync(id);
                return TypedResults.Json(response);
            }).WithName("GetEmployeeById")
            .Produces<EmployeeResponse>()
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithOpenApi();
    }
}