using UnitOfWork.Core.Extension;
using UnitOfWork.Core.Interfaces;
using UnitOfWork.Core.Models;
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
                try
                {
                    var dep = new Department
                    {
                        Name = employee.Department.Name
                    };
                    _unitOfWork.CreateTransaction();
                    await _unitOfWork.DepartmentRespository.AddAsync(dep);
                    _unitOfWork.Save();

                    var emp = new Employee
                    {
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        Age = employee.Age,
                        Email = employee.Email,
                        DepartmentId = dep.Id,
                        DesignationId = 3
                       
                    };
                    await _unitOfWork.EmployeeRespository.AddAsync(emp);

                    var result = _unitOfWork.Save();

                    if (result > 0)
                    {
                        _unitOfWork.Commit();
                        return true;
                    }
                    else
                    {
                        _unitOfWork.Rollback();
                        return false;
                    }
                }
                catch (Exception)
                {
                    _unitOfWork.Rollback();
                }
            }
            return false;
        }

        public async Task<bool> DeleteEmployee(int employeeId)
        {
            if (employeeId > 0)
            {
                var employeeDetails = _unitOfWork.EmployeeRespository.FindByCondition(query =>
                {
                    query = query.Where(x => x.Id == employeeId);

                    return query;
                }).FirstOrDefault();

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

        public async Task<DataTableResponse<EmployeeListResponse>> GetAllEmployees()
        {
            var employeeDetailsList = _unitOfWork.EmployeeRespository.GetAllPaged(query =>
                    {
                        query = query.Select(e => new Employee
                        {
                            FirstName = e.FirstName,
                            LastName = e.LastName,
                            Department = e.Department,
                            DepartmentId = e.DepartmentId,
                            DesignationId = e.DesignationId,
                            Designation = e.Designation
                        });
                        query = query.OrderBy(x => x.FirstName);

                        return query;
                    }, 0, 1);
            
            var dto = employeeDetailsList.Select(x => new EmployeeListResponse
            {
                Name = x.FirstName + x.LastName,
                Department = $"[{x.DepartmentId}] {x.Department.Name}",
                Designation = $"[{x.DesignationId}] {x.Designation.Name}"
            });

            BasePagedInfoResponse basePagedList = new BasePagedInfoResponse
            {
                PageIndex = employeeDetailsList.PageIndex,
                PageSize = employeeDetailsList.PageSize,
                TotalCount = employeeDetailsList.TotalCount,
                TotalPages = employeeDetailsList.TotalPages,
                HasPreviousPage = employeeDetailsList.HasPreviousPage,
                HasNextPage = employeeDetailsList.HasNextPage
            };


            return dto.GetDataTableResponse(basePagedList);
        }

        public async Task<Employee> GetEmployeeById(int employeeId)
        {
            if (employeeId > 0)
            {
                var employee = _unitOfWork.EmployeeRespository.FindByCondition(query =>
                {
                    query = query.Where(x => x.Id == employeeId);

                    return query;
                }).FirstOrDefault();

                if (employee != null)
                {
                    return employee;
                }
            }
            return null;
        }

        public async Task<bool> UpdateEmployee(Employee employeeDetails)
        {
            if (employeeDetails != null)
            {
                var employee = _unitOfWork.EmployeeRespository.FindByCondition(query =>
                {
                    query = query.Where(x => x.Id == employeeDetails.Id);

                    return query;
                }).FirstOrDefault();
                if (employee != null)
                {
                    employee.FirstName = employeeDetails.FirstName;
                    employee.LastName = employeeDetails.LastName;

                    _unitOfWork.EmployeeRespository.Update(employee, x=> x.FirstName, x=> x.LastName);

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
