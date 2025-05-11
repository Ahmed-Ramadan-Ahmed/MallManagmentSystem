namespace MallManagmentSystem.DTOs
{
    public class DebitForRenterDto
    {
        public int Id { get; set; }
        public int RenterId { get; set; }
        public DateTime DebitDate { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; }
    }
}
