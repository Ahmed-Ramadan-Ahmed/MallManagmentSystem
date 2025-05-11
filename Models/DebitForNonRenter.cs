using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MallManagmentSystem.Models
{
    [Table("DebitsForNonRenters", Schema = "dbo")]
    public class DebitForNonRenter
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        public string DebitName { get; set; } // name of the person who gave the debit
        [DataType(DataType.Date)]
        public DateTime DebitDate { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } // e.g., "Utilities", "Maintenance", etc.
        public bool IsActive { get; set; }
        public string Notes { get; set; } // Additional notes about the debit
    }
}
