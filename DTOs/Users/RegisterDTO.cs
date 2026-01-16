using cityWatch_Project.Enums;
using System.ComponentModel.DataAnnotations;

namespace cityWatch_Project.DTOs.Users
{
    public class RegisterDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [EmailAddress(ErrorMessage = "Please enter a valid Email address")]
        public string? Email { get; set; }
        public string? Password { get; set; }
        public District District { get; set; }
        public Province Province { get; set; }
    }
}
