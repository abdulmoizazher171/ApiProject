// User.cs
namespace MyApiProject.Models // <-- This is the namespace you need to reference
{
    public class User 
    { 
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}