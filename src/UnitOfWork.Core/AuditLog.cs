using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitOfWork.Core.Models.Entities;

namespace UnitOfWork.Core
{
    public class AuditLog : BaseEntity
    {
        [AllowNull]
        public string? UserInfo { get; set; } = "Test@gmail.com";

        [AllowNull]
        public string? TableName { get; set; }

        [AllowNull]
        public string? Action { get; set; }

        [AllowNull]
        public string? OriginalValue { get; set; }

        [AllowNull]
        public string? NewValue { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
