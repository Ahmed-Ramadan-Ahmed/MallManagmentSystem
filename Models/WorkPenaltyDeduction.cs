using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MallManagmentSystem.Models
{
    [Table("WorkPenaltyDeductions", Schema = "dbo")]
    public class WorkPenaltyDeduction
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        public int ManagerId { get; set; } // Foreign key to Manager table
        public int EmployeeId { get; set; } // Foreign key to Employee table
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; } // e.g., "Late", "Absent", etc.
        public string Status { get; set; } // e.g., "Pending", "Approved", "Rejected"
        public string Notes { get; set; } // Additional notes about the deduction
        public virtual Employee Employee { get; set; } // Navigation property to Employee
        public virtual Manager Manager { get; set; } // Navigation property to Manager
        public DateTime UpdatedAt { get; internal set; }
    }
}
