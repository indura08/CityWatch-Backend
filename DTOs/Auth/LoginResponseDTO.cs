namespace cityWatch_Project.DTOs.Auth
{
    public class LoginResponseDTO
    {
        public bool Error { get; set; }
        public string Token { get; set; } = null!;
        public string? ErrorMessage { get; set; }

    }
}
