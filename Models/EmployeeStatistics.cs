using System.ComponentModel.DataAnnotations.Schema;

namespace MallManagmentSystem.Models
{
    public class EmployeeStatistics
    {
        public int TotalEmployees { get; set; }
        public int ActiveEmployees { get; set; }
        public Dictionary<string, int> DepartmentDistribution { get; set; }
        public decimal AverageSalary { get; internal set; }
        public decimal TotalSalaryPayments { get; internal set; }
    }
} 