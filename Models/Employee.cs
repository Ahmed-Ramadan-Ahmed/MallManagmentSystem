using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MallManagmentSystem.Models
{
    [Table("Employees", Schema = "dbo")]
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]

        public DateTime CreatedAt { get; internal set; }
        public DateTime UpdatedAt { get; internal set; }
        public string NationalId { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Phone number is required")]
        public string Phone { get; set; }
        [Required]
        public int WorkingHours { get; set; } // Number of hours worked per day
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string WorkLocation { get; set; }
        [Required]
        public decimal Salary { get; set; }
        public string ? SalaryDescription { get; set; } // base + bonus + commission + penalties
        [Required]
        public string JobTitle { get; set; } // e.g., "Manager", "Sales Associate", etc.
        [Required]
        
        public byte[] PersonalImage { get; set; }
        [Required]
        public string PersonalImagePath { get; set; }
        [Required]
        public byte[] NationalIdImage { get; set; }
        [Required]
        public string NationalIdImagePath { get; set; }
        [Required]
        public byte[] CriminalRecord { get; set; } // For storing criminal record documents
        [Required]
        public string CriminalRecordPath { get; set; } // Path to the criminal record document
        [Required]
        public int MallId { get; set; } // Foreign key to Mall table
        public virtual Mall Mall { get; set; } // Navigation property to Mall
        [Required]
        public int EmploymentContractId { get; set; } // Foreign key to Employment Contract table
        public virtual ICollection<EmploymentContract> EmploymentContracts { get; set; } = new HashSet<EmploymentContract>(); // Navigation property to Employment Contracts
        public virtual ICollection<EmployeeLoan> EmployeeLoans { get; set; } = new HashSet<EmployeeLoan>(); // Navigation property to Employee Loans
        public virtual ICollection<Attendance> EmployeeAttendance { get; set; } = new HashSet<Attendance>(); // Navigation property to Store
        public virtual ICollection<WorkPenaltyDeduction> WorkPenaltyDeductions { get; set; } = new HashSet<WorkPenaltyDeduction>(); // Navigation property to Work Penalty Deductions
    }
}
