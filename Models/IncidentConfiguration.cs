using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace cityWatch_Project.Models
{
    internal sealed class IncidentConfiguration : IEntityTypeConfiguration<Incident>
    {
        public void Configure(EntityTypeBuilder<Incident> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(i => i.ReportedBy).WithMany().HasForeignKey(i => i.ReportedByUserId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(i => i.AssignedTo).WithMany().HasForeignKey(i => i.AssignedToUserID).OnDelete(DeleteBehavior.SetNull);
        }
    }
}
