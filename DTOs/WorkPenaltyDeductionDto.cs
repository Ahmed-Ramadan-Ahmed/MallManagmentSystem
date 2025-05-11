namespace MallManagmentSystem.DTOs
{
    public class WorkPenaltyDeductionDto
    {
        public int Id { get; set; }
        public int ManagerId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
    }
}
