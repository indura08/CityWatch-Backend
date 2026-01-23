using cityWatch_Project.Models;

namespace cityWatch_Project.Repositories.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken refreshToken);
        Task<RefreshToken?> GetRefreshTokenAsync(string token);
        Task DeleteRefreshTokenByUserID(int userID);
    }
}
