namespace MallManagmentSystem.DTOs
{
    public class StorePenaltyDto
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public int RenterId { get; set; }
        public DateTime PenaltyDate { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
    }
}
