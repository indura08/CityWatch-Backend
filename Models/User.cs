using cityWatch_Project.Enums;
using System.ComponentModel.DataAnnotations;

namespace cityWatch_Project.Models
{
    public class User
    {
        public Guid UserID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [EmailAddress(ErrorMessage = "Please enter a valid Email address")]
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }

        [Required(ErrorMessage = "Role is requrired")]
        public Role Role { get; set; }
        public District District { get; set; }
        public Province Province { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    }
}
