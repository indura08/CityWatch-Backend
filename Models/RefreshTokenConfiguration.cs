using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace cityWatch_Project.Models
{
    internal sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder) 
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Token);
            
            builder.HasIndex(r => r.Token).IsUnique();
            
            builder.HasOne(r => r.user).WithMany().HasForeignKey(r => r.UserID);
        }
    }
}
