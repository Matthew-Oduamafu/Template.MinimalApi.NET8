using Microsoft.AspNetCore.Mvc;
using Template.MinimalApi.NET8.Models;
using Template.MinimalApi.NET8.Models.Dtos;
using Template.MinimalApi.NET8.Services.Interfaces;

namespace Template.MinimalApi.NET8.Extensions.EndpointsExtensions;

public static class EmployeeEndpoints
{
    public static void MapEmployeeEndpoints(this IEndpointRouteBuilder routes, ILinkService linkService)
    {
        var group = routes.MapGroup("/api/employees")
            .WithTags("Employees");

        group.MapGet("",
                async ([FromServices] IEmployeeService employeeService, [FromQuery] int page,
                    [FromQuery] int pageSize) =>
                {
                    var filter = new BaseFilter { Page = page, PageSize = pageSize };
                    var response = await employeeService.GetEmployees(filter);
                    AddLinksForPagedEmployee(response, filter, linkService);
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
                AddLinksForEmployee(response, linkService);
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
    
    private static void AddLinksForEmployee(IGenericApiResponse<EmployeeResponse> apiResponse, ILinkService linkService)
    {
        apiResponse.Data?.Links.Add(
            linkService.GenerateLink("GetEmployeeById", new { id = apiResponse.Data.Id }, "self", "GET"));
        /***
        apiResponse.Data?.Links.Add(
            linkService.GenerateLink(nameof(Update), new { id = apiResponse.Data.Id }, "update-employee", "PUT"));
        apiResponse.Data?.Links.Add(
            linkService.GenerateLink(nameof(Delete), new { id = apiResponse.Data.Id }, "delete-employee", "DELETE"));
            */
    }

    private static void AddLinksForPagedEmployee(IGenericApiResponse<PagedList<EmployeeResponse>> apiResponse, BaseFilter filter, ILinkService linkService)
    {
        if (apiResponse.Data?.Items == null || !apiResponse.Data.Items.Any()) return;

        apiResponse.Data.Links.Add(
            linkService.GenerateLink("GetEmployees",
                new { Page = filter.Page, PageSize = filter.PageSize }, "self", "GET"));
        
        if (apiResponse.Data.Page > 1)
        {
            apiResponse.Data.Links.Add(
                linkService.GenerateLink("GetEmployees",
                    new { Page = filter.Page - 1, PageSize = filter.PageSize }, "previous-page", "GET"));
        }

        if (apiResponse.Data.Page < apiResponse.Data.TotalPages)
        {
            apiResponse.Data.Links.Add(
                linkService.GenerateLink("GetEmployees",
                    new { Page = filter.Page + 1, PageSize = filter.PageSize }, "next-page", "GET"));
        }
    }
}