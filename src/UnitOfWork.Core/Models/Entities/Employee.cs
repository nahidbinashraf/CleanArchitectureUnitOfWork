using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace UnitOfWork.Core.Models.Entities
{
    public class Employee : BaseEntity
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Age { get; set; }

        [AllowNull]
        public string? Email { get; set; }

        [Required]
        public long DepartmentId { get; set; }
        public Department Department { get; set; }

        [Required]
        public long DesignationId { get; set; }
        public Designation Designation { get; set; }
    }
}
