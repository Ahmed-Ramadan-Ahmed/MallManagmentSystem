using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MallManagmentSystem.Models
{
    [Table("EmployeeLoans", Schema = "dbo")]
    public class EmployeeLoan
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        public int EmployeeId { get; set; } // Foreign key to Employee table
        [DataType(DataType.Date)]
        public DateTime LoanDate { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } // e.g., "Salary Advance", "Emergency Loan", etc.
        public string Status { get; set; } // e.g., "Pending", "Approved", "Rejected"
        public string Notes { get; set; } // Additional notes about the loan
        public virtual Employee Employee { get; set; } // Navigation property to Employee
        public DateTime CreatedAt { get; internal set; }
        public DateTime UpdatedAt { get; internal set; }
        public bool IsActive { get; set; } // Indicates if the loan is currently active

    }
}
