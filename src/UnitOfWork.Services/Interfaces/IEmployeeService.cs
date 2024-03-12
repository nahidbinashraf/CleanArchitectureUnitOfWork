using UnitOfWork.Core.Models;
using UnitOfWork.Core.Models.Entities;
using UnitOfWork.Services.Models.Response;

namespace UnitOfWork.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<bool> CreateEmployee(Employee employee);

        Task<DataTableResponse<EmployeeListResponse>> GetAllEmployees();

        Task<Employee> GetEmployeeById(int employeeId);

        Task<bool> UpdateEmployee(Employee employee);

        Task<bool> DeleteEmployee(int employeeId);
    }
}
