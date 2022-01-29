using ImMicro.Model.AuditLog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ImMicro.Data.Configurations
{
    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.ToTable(nameof(AuditLog));

            builder.HasKey(o => o.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}
