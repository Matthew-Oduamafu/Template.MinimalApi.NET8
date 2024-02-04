using Template.MinimalApi.NET8.Models;
using Template.MinimalApi.NET8.Models.Dtos;

namespace Template.MinimalApi.NET8.Services.Interfaces;

public interface IEmployeeService
{
    Task<IGenericApiResponse<EmployeeResponse>> GetEmployeeByIdAsync(string id);
    Task<IGenericApiResponse<IReadOnlyList<EmployeeResponse>>> GetEmployeesAsync();
    Task<IGenericApiResponse<PagedList<EmployeeResponse>>> GetEmployees(BaseFilter filter);
    Task<IGenericApiResponse<EmployeeResponse>> AddEmployeeAsync(EmployeeRequest employeeRequest);
    Task<IGenericApiResponse<EmployeeResponse>> UpdateEmployeeAsync(string id, EmployeeRequest employeeRequest);
    Task<IGenericApiResponse<EmployeeResponse>> DeleteEmployeeAsync(string id);
}