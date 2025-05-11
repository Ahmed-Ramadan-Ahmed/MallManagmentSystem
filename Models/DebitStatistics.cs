using System.ComponentModel.DataAnnotations.Schema;

namespace MallManagmentSystem.Models
{
    public class DebitStatistics
    {
        public int TotalDebits { get; set; }
        public int ActiveDebits { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalDebitAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPaidAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AverageDebitAmount { get; set; }
    }
} 