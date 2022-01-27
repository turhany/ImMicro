using AutoMapper;
using ImMicro.Contract.Audit;
using ImMicro.Model.AuditLog;

namespace ImMicro.Contract.Mappings.AutoMapper
{
    public class AuditLogMapping : Profile
    {
        public AuditLogMapping()
        {
            CreateMap<AuditLog, AuditLogView>();
        }
    }
}