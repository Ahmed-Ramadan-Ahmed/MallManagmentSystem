using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MallManagmentSystem.Models
{
    [Table("DebitsForRenters", Schema = "dbo")]
    public class DebitForRenter
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        public int RenterId { get; set; } // Foreign key to Renter table
        [DataType(DataType.Date)]
        public DateTime DebitDate { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } // e.g., "Rent", "Utilities", etc. 
        public bool IsActive { get; set; }
        public string Notes { get; set; } // Additional notes about the debit
        public virtual Store Store { get; set; } // Navigation property to Renter
    }
}
