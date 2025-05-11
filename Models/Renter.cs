using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MallManagmentSystem.Models
{
    [Table("Renters", Schema = "dbo")]
    public class Renter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        public string NationalId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; } // Indicates if the Renter is currently active
        public byte[] PersonalImage { get; set; } // Image of the Renter
        public string PersonalImagePath { get; set; } // Path to the image file
        public byte[] NationalIdImage { get; set; }
        public string NationalIdImagePath { get; set; } // Path to the image file

        public virtual ICollection<Store> Stores { get; set; } // Navigation property to Shops        
        public DateTime CreatedAt { get; internal set; }
        public DateTime UpdatedAt { get; internal set; }
    }
}
