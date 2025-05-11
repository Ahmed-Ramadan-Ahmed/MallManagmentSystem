using System.ComponentModel.DataAnnotations.Schema;

namespace MallManagmentSystem.Models
{
    public class StoreStatistics
    {
        public int TotalStores { get; set; }
        public int OccupiedStores { get; set; }
        public int VacantStores { get; set; }
        public Dictionary<string, int> StoreCategories { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AverageRentPerStore { get; set; }
        public object OccupancyRate { get; internal set; }
    }
} 