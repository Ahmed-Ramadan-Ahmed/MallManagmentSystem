using System.ComponentModel.DataAnnotations.Schema;

namespace MallManagmentSystem.Models
{
    [Table("Malls", Schema = "dbo")]
    public class Mall
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int NumberOfStores { get; set; }
        public int NumberOfActiveStores { get; set; }
        public int NumberOfFloors { get; set; }
        public int NumberOfEmployees { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalExpenses { get; set; }
        public bool IsActive { get; set; }
        public bool IsCenter { get; set; } // Indicates if the Building is a small Center mall

        public virtual ICollection<Store> Stores { get; set; } = new HashSet<Store>(); // Navigation property to Store
        public virtual ICollection<Employee> Employees { get; set; } = new HashSet<Employee>(); // Navigation property to Employee
    }
}
