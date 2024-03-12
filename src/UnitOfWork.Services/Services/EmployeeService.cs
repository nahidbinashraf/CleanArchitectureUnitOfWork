using UnitOfWork.Core.Interfaces;
using UnitOfWork.Core.Models.Entities;
using UnitOfWork.Services.Interfaces;
using UnitOfWork.Services.Models.Response;

namespace UnitOfWork.Services.Services
{
    public class EmployeeService : IEmployeeService
    {
        public IUnitOfWork _unitOfWork;

        public EmployeeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateEmployee(Employee employee)
        {
            if (employee != null)
            {
                await _unitOfWork.EmployeeRespository.AddAsync(employee);

                var result = _unitOfWork.Save();

                if (result > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }

        public async Task<bool> DeleteEmployee(int employeeId)
        {
            if (employeeId > 0)
            {
                var employeeDetails = _unitOfWork.EmployeeRespository.GetById(employeeId);
                if (employeeDetails != null)
                {
                    _unitOfWork.EmployeeRespository.Delete(employeeDetails);
                    var result = _unitOfWork.Save();

                    if (result > 0)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }

        public async Task<IEnumerable<EmployeeListResponse>> GetAllEmployees()
        {
            var employeeDetailsList = _unitOfWork.EmployeeRespository.GetAll(query =>
                    query.Select(e=> new Employee
                    {
                        FirstName = e.FirstName, 
                        LastName = e.LastName,
                        Department = e.Department,
                        DepartmentId = e.DepartmentId,
                        DesignationId = e.DesignationId,
                        Designation = e.Designation
                    })
            );

            var dto = employeeDetailsList.Select(x => new EmployeeListResponse
            {
                Name = x.FirstName + x.LastName,
                Department = $"[{x.DepartmentId}] {x.Department.Name}",
                Designation = $"[{x.DesignationId}] {x.Designation.Name}"
            });

            return dto;
        }

        public async Task<Employee> GetEmployeeById(int employeeId)
        {
            if (employeeId > 0)
            {
                var EmployeeDetails = _unitOfWork.EmployeeRespository.GetAll().FirstOrDefault();
                if (EmployeeDetails != null)
                {
                    return EmployeeDetails;
                }
            }
            return null;
        }

        public async Task<bool> UpdateEmployee(Employee employeeDetails)
        {
            if (employeeDetails != null)
            {
                var employee = _unitOfWork.EmployeeRespository.GetById(employeeDetails.Id);
                if (employee != null)
                {
                    employee.FirstName = employeeDetails.FirstName;

                    _unitOfWork.EmployeeRespository.Update(employee);

                    var result = _unitOfWork.Save();

                    if (result > 0)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }
    }
}
