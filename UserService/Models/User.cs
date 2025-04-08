// user Model


using System.ComponentModel.DataAnnotations;


namespace UserService.Models
{
    public class User
    {
        // [Key]
        // public int Id { get; set; }
        // [Required]
        // public string Username { get; set; }
        // [Required]
        // public string PasswordHash { get; set; }
        // [Required]
        // public string Email { get; set; }
        // public bool IsAdmin { get; set; }
        // public DateTime CreatedAt { get; set; }
        // public DateTime? LastLogin { get; set; }

        [Key]

        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public UserRole Role { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
}