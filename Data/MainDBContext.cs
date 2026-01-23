using cityWatch_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace cityWatch_Project.Data
{
    public class MainDBContext : DbContext
    {
        public MainDBContext(DbContextOptions<MainDBContext> options): base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    }
}
