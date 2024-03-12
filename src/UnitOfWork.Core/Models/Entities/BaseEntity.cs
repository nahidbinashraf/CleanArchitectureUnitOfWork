using System.ComponentModel.DataAnnotations;

namespace UnitOfWork.Core.Models.Entities
{
    public abstract class BaseEntity
    {
        [Required]
        public long Id { get; set; }
    }
}
