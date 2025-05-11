using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MallManagmentSystem.Models
{
    [Table("StoreRentContracts", Schema = "dbo")]
    public class StoreRentContract
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        public int RenterId { get; set; } 
        public int StoreId { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal MonthlyRent { get; set; }
        public decimal DepositAmount { get; set; } // Security deposit amount
        public bool IsActive { get; set; }
        public string Status { get; set; } // e.g., "Active", "Expired", "Cancelled"
        public Byte[]? ContractDocument { get; set; } // For storing contract documents
        public Store Store { get; set; } // Navigation property to Store
        public Renter Renter { get; set; } // Navigation property to Renter
        public DateTime UpdatedAt { get; internal set; }
    }
}
