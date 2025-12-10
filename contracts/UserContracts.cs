using System.ComponentModel.DataAnnotations;
namespace MyApiProject.contracts;

public class UserDto
    {
        

        // Input/User Fields
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        // INPUT FIELD: Plaintext password for hashing in the service layer
        // This replaces PasswordHash and PasswordSalt in the DTO structure.
        [Required]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty; 

        // Status and Metadata (Output/DB Generated)

      
        
        // Navigation Collection (Output/DB Reference)
        
    }