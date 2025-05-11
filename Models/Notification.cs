using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace MallManagmentSystem.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Type { get; set; } // ContractExpiry, PaymentOverdue, Absence, etc.

        [Required]
        public string Title { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? ReadAt { get; set; }

        public bool IsRead { get; set; }

        public string Severity { get; set; } // Info, Warning, Critical

        public string RecipientType { get; set; } // Employee, Renter, Admin, etc.

        public int? RecipientId { get; set; }

        public string RelatedEntityType { get; set; } // Contract, Payment, Attendance, etc.

        public int? RelatedEntityId { get; set; }

        [NotMapped]
        public Dictionary<string, string> Metadata { get; set; }

        // Store the metadata as JSON in the database
        [Column("Metadata")]
        public string MetadataJson
        {
            get => Metadata == null ? null : JsonSerializer.Serialize(Metadata);
            set => Metadata = string.IsNullOrEmpty(value) ? null : JsonSerializer.Deserialize<Dictionary<string, string>>(value);
        }
    }
} 