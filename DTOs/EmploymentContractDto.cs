namespace MallManagmentSystem.DTOs
{
    public class EmploymentContractDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Location { get; set; }
        public string JobDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DurationInMonths { get; set; }
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; }
        public DebitForNonRenterDto MapToDto(DebitForNonRenter model)
        {
            return new DebitForNonRenterDto
            {
                Id = model.Id,
                DebitName = model.DebitName,
                DebitDate = model.DebitDate,
                Amount = model.Amount,
                Description = model.Description,
                IsActive = model.IsActive,
                Notes = model.Notes
            };
        }
    }
}
