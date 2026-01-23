using cityWatch_Project.Data;
using cityWatch_Project.Helpers;
using cityWatch_Project.Models;
using cityWatch_Project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace cityWatch_Project.Repositories.Implementations
{
    public class RefreshTokenRepositroy : IRefreshTokenRepository
    {
        private readonly MainDBContext _dbContext;
        private readonly JwtTokenGenerator _jwtTokenGenerator;
        public RefreshTokenRepositroy(MainDBContext dbContext, JwtTokenGenerator jwtTokenGenerator)
        {
            _dbContext = dbContext;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task AddAsync(RefreshToken refreshToken)
        {

            _dbContext.RefreshTokens.Add(refreshToken);
            await _dbContext.SaveChangesAsync();

        }

        public async Task DeleteRefreshTokenByUserID(int userID)
        {
            await _dbContext.RefreshTokens.Where(r => r.UserID == userID).ExecuteDeleteAsync();
        }

        public async Task<RefreshToken> GetRefreshTokenAsync(string token)
        {
            return await _dbContext.RefreshTokens.Include(r => r.User).FirstOrDefaultAsync(r => r.Token == token);
        }
    }
}
