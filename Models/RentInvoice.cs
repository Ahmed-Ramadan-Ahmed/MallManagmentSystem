using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MallManagmentSystem.Models
{
    [Table("RentInvoices", Schema = "dbo")]
    public class RentInvoice
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StoreId { get; set; }

        [Required]
        public int RenterId { get; set; }

        [Required]
        public DateTime InvoiceDate { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        public decimal RemainAmount { get { return Amount - DebitAmount; } } // Remaining amount to be paid

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal DebitAmount { get; set; } = 0;

        public bool IsPending { get; set; } = true; // Pending, PartiallyPaid, Paid, Overdue
        public bool Isoverdue { get { return (DateTime.UtcNow.Date > DueDate && IsPending);  } }
        public string Notes { get; set; }

        // Navigation properties
        [ForeignKey("StoreId")]
        public Store Store { get; set; }

        [ForeignKey("RenterId")]
        public Renter Renter { get; set; }
    }
}
