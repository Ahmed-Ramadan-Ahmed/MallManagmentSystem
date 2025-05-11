using System.ComponentModel.DataAnnotations.Schema;

namespace MallManagmentSystem.Models
{
    [Table("Stores", Schema = "dbo")]
    public class Store
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int FloorNumber { get; set; }
        public int NumberOfEmployees { get; set; }
        public bool IsActive { get; set; }
        public int OwnerId { get; set; } // Foreign key to Owner table
        public int MallId { get; set; } // Foreign key to Mall table
        public int ContractId { get; set; } // Foreign key to Contract table
        public decimal MonthlyRentAmount { get; set; } // Monthly rent amount
        public virtual Mall Mall { get; set; } // Navigation property to Mall
        public virtual ICollection<StoreRentContract> Contracts { get; set; } // Navigation property to Contracts
        public virtual ICollection<Renter> Renters { get; set; } // Navigation property to Renters

    }
}
