using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ImMicro.Model;

namespace ImMicro.Data.Configurations
{
    public class RequestLogConfiguration : IEntityTypeConfiguration<RequestLog>
    {
        public void Configure(EntityTypeBuilder<RequestLog> builder)
        {
            builder.HasKey(o => o.Id);
        }
    }
}