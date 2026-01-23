namespace cityWatch_Project.DTOs.Auth
{
    public class LoginServiceResponse
    {
        public string? Token { get; set; }
        public bool Error { get; set; }
        public string? ErrorMessage { get; set; }
        public string? RefreshToken { get; set; }
    }
}
