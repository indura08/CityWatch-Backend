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

    }
}
