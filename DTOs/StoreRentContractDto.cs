namespace MallManagmentSystem.DTOs
{
    public class StoreRentContractDto
    {
        public int Id { get; set; }
        public int RenterId { get; set; }
        public int StoreId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal MonthlyRent { get; set; }
        public decimal DepositAmount { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; }
    }
}
