using System.ComponentModel.DataAnnotations.Schema;

namespace MallManagmentSystem.Models
{
    public class LoanStatistics
    {
        public int TotalLoans { get; set; }
        public int ActiveLoans { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalLoanAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPaidAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AverageLoanAmount { get; set; }
    }
} 