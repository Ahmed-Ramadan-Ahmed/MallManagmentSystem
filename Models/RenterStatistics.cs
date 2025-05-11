using System.ComponentModel.DataAnnotations.Schema;

namespace MallManagmentSystem.Models
{
    public class RenterStatistics
    {
        public int TotalRenters { get; set; }
        public int ActiveRenters { get; set; }
        public int TotalContracts { get; set; }
        public int ActiveContracts { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalRentIncome { get; set; }
        public decimal TotalRentDue { get; internal set; }
        public decimal TotalRentOverdue { get; internal set; }
        public decimal AverageRent { get; internal set; }
    }
} 