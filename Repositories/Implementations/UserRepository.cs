using cityWatch_Project.Data;
using cityWatch_Project.Models;
using cityWatch_Project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace cityWatch_Project.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly MainDBContext _dbContext;

        public UserRepository(MainDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddUserAsync(User user)
        {
            if (user == null) return;

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<User> FindUserByEmail(string email)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Email == email);
            if (user == null) return null!;

            return user;
        }
    }
}
