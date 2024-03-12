using System.ComponentModel.DataAnnotations;

namespace UnitOfWork.Core.Models.Entities
{
    public class Designation : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        public List<Employee> Employees { get; set; } = new List<Employee>();
    }
}
