using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MallManagmentSystem.Models
{
    [Table("StorePenalties", Schema = "dbo")]
    public class StorePenalty
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public int StoreId { get; set; }
        public int RenterId { get; set; } 

        [DataType(DataType.Date)]
        public DateTime PenaltyDate { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; } // e.g., "Late Payment", "Contract Violation", etc.
        public string Status { get; set; } // e.g., "Pending", "Paid", "Waived"
        public string Notes { get; set; } // Additional notes about the penalty

        public virtual Store Store { get; set; } // Navigation property to Store
        public virtual Renter Renter { get; set; } // Navigation property to Renter
        public DateTime CreatedAt { get; internal set; }
        public DateTime UpdatedAt { get; internal set; }
    }
}
