using cityWatch_Project.Models;

namespace cityWatch_Project.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> FindUserByEmail(string email);
        Task AddUserAsync(User user);
        
    }
}
