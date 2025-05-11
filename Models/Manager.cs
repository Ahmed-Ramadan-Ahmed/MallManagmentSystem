using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MallManagmentSystem.Models
{
    [Table("Managers", Schema = "dbo")]
    public class Manager
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        
        public string NationalId { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public string Phone { get; set; }
        public bool HasFullAccess { get; set; }
        public bool IsActive { get; set; }
        public byte[] PersonalImage { get; set; } // Image of the Renter
        public string PersonalImagePath { get; set; } // Path to the image file
        public byte[] NationalIdImage { get; set; }
        public string NationalIdImagePath { get; set; } // Path to the image file
    }
}
