using Mapster;
using Template.MinimalApi.NET8.Data.Entities;
using Template.MinimalApi.NET8.Extensions;
using Template.MinimalApi.NET8.Models;
using Template.MinimalApi.NET8.Models.Dtos;
using Template.MinimalApi.NET8.Repositories.Interfaces;
using Template.MinimalApi.NET8.Services.Interfaces;

namespace Template.MinimalApi.NET8.Services.Providers;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ILogger<EmployeeService> _logger;
    private readonly IPgRepository _pgRepository;

    public EmployeeService(IEmployeeRepository employeeRepository, IPgRepository pgRepository,
        ILogger<EmployeeService> logger)
    {
        _employeeRepository = employeeRepository;
        _pgRepository = pgRepository;
        _logger = logger;
    }

    public async Task<IGenericApiResponse<EmployeeResponse>> GetEmployeeByIdAsync(string id)
    {
        try
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (employee == null) return GenericApiResponse<EmployeeResponse>.Default.ToNotFoundApiResponse();

            return employee.Adapt<EmployeeResponse>().ToOkApiResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting employee by id {Id}", id);
            return GenericApiResponse<EmployeeResponse>.Default.ToInternalServerErrorApiResponse();
        }
    }

    public async Task<IGenericApiResponse<IReadOnlyList<EmployeeResponse>>> GetEmployeesAsync()
    {
        try
        {
            var employees = await _employeeRepository.GetEmployeesAsync();
            if (employees.Count == 0)
                return GenericApiResponse<IReadOnlyList<EmployeeResponse>>.Default.ToNotFoundApiResponse();

            return employees.Adapt<IReadOnlyList<EmployeeResponse>>().ToOkApiResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting employees");
            return GenericApiResponse<IReadOnlyList<EmployeeResponse>>.Default.ToInternalServerErrorApiResponse();
        }
    }

    public async Task<IGenericApiResponse<PagedList<EmployeeResponse>>> GetEmployees(BaseFilter filter)
    {
        try
        {
            var employees = await _employeeRepository.GetEmployeesAsQueryable()
                .ProjectToType<EmployeeResponse>()
                .ToPagedList(filter.Page, filter.PageSize);

            return employees.ToOkApiResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting employees");
            return GenericApiResponse<PagedList<EmployeeResponse>>.Default.ToInternalServerErrorApiResponse();
        }
    }

    public async Task<IGenericApiResponse<EmployeeResponse>> AddEmployeeAsync(EmployeeRequest employeeRequest)
    {
        try
        {
            var employee = employeeRequest.Adapt<Employee>();
            await _employeeRepository.AddEmployeeAsync(employee);
            await _pgRepository.SaveChangesAsync();
            return employee.Adapt<EmployeeResponse>().ToOkApiResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while adding employee");
            return GenericApiResponse<EmployeeResponse>.Default.ToInternalServerErrorApiResponse();
        }
    }

    public async Task<IGenericApiResponse<EmployeeResponse>> UpdateEmployeeAsync(string id, EmployeeRequest employeeRequest)
    {
        try
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (employee == null) return GenericApiResponse<EmployeeResponse>.Default.ToNotFoundApiResponse();

            employee = employeeRequest.Adapt(employee);
            _employeeRepository.UpdateEmployeeAsync(employee);
            await _pgRepository.SaveChangesAsync();
            return employee.Adapt<EmployeeResponse>().ToOkApiResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating employee");
            return GenericApiResponse<EmployeeResponse>.Default.ToInternalServerErrorApiResponse();
        }
    }

    public async Task<IGenericApiResponse<EmployeeResponse>> DeleteEmployeeAsync(string id)
    {
        try
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (employee == null) return GenericApiResponse<EmployeeResponse>.Default.ToNotFoundApiResponse();

            _employeeRepository.DeleteEmployeeAsync(employee);
            await _pgRepository.SaveChangesAsync();
            return employee.Adapt<EmployeeResponse>().ToOkApiResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting employee");
            return GenericApiResponse<EmployeeResponse>.Default.ToInternalServerErrorApiResponse();
        }
    }
}