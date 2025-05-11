using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MallManagmentSystem.Models
{
    [Table("Attendances", Schema = "dbo")]
    public class Attendance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int EmployeeId { get; set; } // Foreign key to Employee table
        [DataType(DataType.Date)]
        public DateTime CheckInTime { get; set; }
        [DataType(DataType.Date)]
        public DateTime CheckOutTime { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public bool IsPresent { get; set; } // Indicates if the employee was present
        public string Status { get; set; } // e.g., "Present", "Absent", "Leave"
        public string Notes { get; set; } // Additional notes about the attendance
        public virtual Employee Employee { get; set; }
        public DateTime CreatedAt { get; internal set; }
        public DateTime UpdatedAt { get; internal set; }
    }
}
