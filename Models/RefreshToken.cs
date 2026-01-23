namespace cityWatch_Project.Models
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string? Token { get; set; }
        public int UserID { get; set; }
        public DateTime ExpiresOn { get; set; }
        public User? User { get; set; }

    }
}
