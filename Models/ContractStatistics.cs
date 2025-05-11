using System.ComponentModel.DataAnnotations.Schema;

namespace MallManagmentSystem.Models
{
    public class ContractStatistics
    {
        public int TotalContracts { get; set; }
        public int ActiveContracts { get; set; }
        public int ExpiredContracts { get; set; }
        public double AverageContractDuration { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalContractValue { get; set; }
        public decimal ExpiringContracts { get; internal set; }
        public decimal RenewalRate { get; internal set; }
    }
} 