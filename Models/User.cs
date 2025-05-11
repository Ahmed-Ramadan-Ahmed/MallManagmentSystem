using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MallManagmentSystem.Models
{
    /// <summary>
    /// Represents a user in the mall management system.
    /// This model stores user information and authentication details.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the username used for login.
        /// Must be unique and between 3-50 characters.
        /// </summary>
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the user's email address.
        /// Must be unique and in a valid email format.
        /// </summary>
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the hashed password.
        /// The actual password is never stored in plain text.
        /// </summary>
        [Required]
        public string PasswordHash { get; set; }

        /// <summary>
        /// Gets or sets the user's role in the system.
        /// Determines the user's permissions and access levels.
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets the user's first name.
        /// </summary>
        [StringLength(50)]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the user's last name.
        /// </summary>
        [StringLength(50)]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the user's phone number.
        /// </summary>
        [Phone]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets whether the user account is active.
        /// Inactive accounts cannot log in to the system.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets the date and time when the user account was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the date and time of the user's last login.
        /// Null if the user has never logged in.
        /// </summary>
        public DateTime? LastLoginAt { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the user account was last modified.
        /// </summary>
        public DateTime? LastModifiedAt { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who last modified this account.
        /// </summary>
        public int? LastModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the collection of refresh tokens associated with this user.
        /// Used for maintaining user sessions.
        /// </summary>
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }

        /// <summary>
        /// Gets the user's full name by combining first and last name.
        /// </summary>
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}".Trim();
    }
} 