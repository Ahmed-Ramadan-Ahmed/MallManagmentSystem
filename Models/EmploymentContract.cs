using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MallManagmentSystem.Models
{
    [Table("EmploymentContracts", Schema = "dbo")]
    public class EmploymentContract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int EmployeeId { get; set; } // Foreign key to Employee table
        [Required]
        public string Location { get; set; } // Location of the job
        [Required]
        public string JobDescription { get; set; } // e.g., "Manager", "Sales Associate", etc.
        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public int DurationInMonths { get; set; } // Duration of the contract in months

        [Required]
        public decimal Salary { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public string Status { get; set; } // e.g., "Active", "Expired", "Cancelled"
        [Required]
        public Byte[]? ContractDocument { get; set; } // For storing contract documents
        [Required]
        public string ContractDocumentPath { get; set; } // Path to the contract document

        public virtual Employee ? Employee { get; set; } // Navigation property to Employee
        public DateTime UpdatedAt { get; internal set; }
    }
}
