using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ImMicro.Model.RequestLog;

namespace ImMicro.Data.Configurations
{
    public class RequestLogConfiguration : IEntityTypeConfiguration<RequestLog>
    {
        public void Configure(EntityTypeBuilder<RequestLog> builder)
        {
            builder.ToTable(nameof(RequestLog));

            builder.HasKey(o => o.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}