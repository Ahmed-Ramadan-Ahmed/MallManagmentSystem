using System;
using System.ComponentModel.DataAnnotations;

namespace MallManagmentSystem.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }

        [Required]
        public string Token { get; set; }

        public DateTime ExpiryDate { get; set; }

        public bool IsRevoked { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string CreatedByIp { get; set; }

        public DateTime? RevokedAt { get; set; }

        public string RevokedByIp { get; set; }

        public string ReplacedByToken { get; set; }

        public string ReasonRevoked { get; set; }

        // Foreign key
        public int UserId { get; set; }

        // Navigation property
        public virtual User User { get; set; }
    }
} 