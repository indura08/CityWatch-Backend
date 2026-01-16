using cityWatch_Project.DTOs.Auth;
using cityWatch_Project.DTOs.Users;
using cityWatch_Project.Enums;

namespace cityWatch_Project.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginServiceResponse> RegisterAsync(RegisterDTO registerDto, Role roleType);
        Task<LoginServiceResponse> LoginAsync(LoginDto logingDto);
    }
}
