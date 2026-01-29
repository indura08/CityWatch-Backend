using cityWatch_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace cityWatch_Project.Data
{
    public class MainDBContext : DbContext
    {
        public MainDBContext(DbContextOptions<MainDBContext> options): base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<Incident> Incident => Set<Incident>();

        //me cofiguration ek DBcontext class ekt daanna one nattnm wenna api wenma confre clas walin configure krna kisima deyk hadenne nha database eke
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MainDBContext).Assembly);
        }
    }
}
