using UnitOfWork.Core.Models;

namespace UnitOfWork.Services.Models.Response
{
    public class EmployeeListResponse
    {
        public string Name { get; set; }

        public string Age { get; set; }

        public string Email { get; set; }

        public string Department { get; set; }

        public string Designation { get; set; }
    }
}
